using goofygame.enviroment.interactable;
using goofygame.inventory;
using goofygame.inventory.gun;
using System;
using System.Collections;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace goofygame.creature.player {
    public class Player : Creature {
        [SerializeField] TextMeshProUGUI _healthDisplay;

        [SerializeField] GameObject _healTimerIndicator;
        Slider _healingIndicatorSlider;

        [SerializeField] Image _hand;
        [SerializeField] Image _damageEffect;

        private Item _activeItem;

        public event Action<int> SwitchItemEvent;
        public event Action<WeaponItem> ShootEvent;

        private bool _isAttacking = false;

        public Inventory inventory = new Inventory();

        private float _healthTimer, _damageTicker, _handTicker;

        private void Awake() {
            healEvent += UpdateHealth;
            damageEvent += UpdateHealth;
            SwitchItemEvent += SwitchItem;
            UpdateHealth(Health);
            _healingIndicatorSlider = _healTimerIndicator.GetComponentInChildren<Slider>();
            _healthTimer = 0;
            _damageTicker = 1f;
            _handTicker = 1f;
        }

        private void UpdateHealth(int health) {
            StringBuilder healthText = new StringBuilder("[ Health: ").Append(Health).Append(" ]");
            _healthDisplay.text = Health > 0 ? healthText.ToString() : "";
        }

        private void Update() {

            if(_damageTicker < 1f) {
                _damageTicker += 2.5f * Time.deltaTime;
                _damageEffect.color = Health > 0 ? new Color(255, 255, 255, Mathf.Lerp(0.75f, 0, _damageTicker)) : new Color(255, 255 , 255, 0.75f);
            }

            if(_handTicker < 1f && _activeItem != null) {
                _handTicker += _activeItem.ItemCooldown * Time.deltaTime;

                _hand.rectTransform.position = new Vector3(
                    _hand.rectTransform.position.x,
                     Mathf.Lerp(-150, 250, _handTicker),
                    _hand.rectTransform.position.z);
            }

            if(Input.GetKeyDown(KeyCode.T)) {
                Debug.Log(_hand.rectTransform.position.y);
            }

            if(Input.GetKey(KeyCode.Mouse0)) {
                if(_activeItem is WeaponItem _gunItem) {
                    if(!_isAttacking) {
                        StartCoroutine(PlayerAttack(_gunItem));
                    }
                }

                if(_activeItem == ItemRegistry.medkid && !(Health == _maxHealth)) {
                    _hand.sprite = _activeItem.ActiveSprite;
                    _healTimerIndicator.SetActive(true);
                    _healthTimer += 0.5f * Time.deltaTime;
                    _healingIndicatorSlider.value = _healthTimer;
                    if(_healthTimer >= 1.5f) {
                        Heal(5);
                        _healthTimer = 0;
                        _hand.sprite = _activeItem.NormalSprite;
                        inventory.removeItem(ItemRegistry.medkid);
                        SwitchItemEvent.Invoke(0);
                    }
                }
            }

            if(Input.GetKeyUp(KeyCode.Mouse0)) {
                _healTimerIndicator.SetActive(false);
                if(_activeItem != null) { _hand.sprite = _activeItem.NormalSprite; }
                _healthTimer = 0;
                _healingIndicatorSlider.value = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                inventory.addItem(new ItemStack(ItemRegistry.medkid));
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                inventory.addItem(new ItemStack(ItemRegistry.handgun));
            }

            if (Input.GetKeyDown(KeyCode.K)) {
                inventory.addItem(new ItemStack(ItemRegistry.theBigBaller));
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                Interact();
            }

            if(_activeItem != null && Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.Q)) {
                var item = Instantiate(Resources.Load("prefabs/Item"), transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
                item.GetComponent<Collectable>().Item = _activeItem;
                inventory.removeItem(_activeItem);
                SwitchItemEvent.Invoke(0);
            }



            // ugly piece of shit code, please forgive me all lords of programming but i don't know
            // how to write this in a better, less dogshit and cleaner way

            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                SwitchItemEvent.Invoke(0);
            } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
                SwitchItemEvent.Invoke(1);
            } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
                SwitchItemEvent.Invoke(2);
            } else if(Input.GetKeyDown(KeyCode.Alpha4)) {
                SwitchItemEvent.Invoke(3);
            } else if(Input.GetKeyDown(KeyCode.Alpha5)) {
                SwitchItemEvent.Invoke(4);
            } else if(Input.GetKeyDown(KeyCode.Alpha6)) {
                SwitchItemEvent.Invoke(5);
            }
        }

        public override void Damage(int amount = 1) {
            base.Damage(amount);
            _damageTicker = 0;
        }

        private void SwitchItem(int itemIndex) {
            try {
                _activeItem = inventory.Container[itemIndex].Item;
            } catch (ArgumentOutOfRangeException) {
                _activeItem = null;
                _hand.sprite = null;
                _hand.color = new Color(255, 255, 255, 0);
                return;
            }
            _handTicker = 0;
            _hand.color = Color.white;
            _hand.sprite = _activeItem.NormalSprite;
        }

        public bool Interact() {
            RaycastHit _hit;
            Physics.Raycast(head.position, head.forward, out _hit, 5);

            if(_hit.transform != null && _hit.collider.isTrigger) {
                var interactable = _hit.transform.gameObject.GetComponent<IInteractable>();
                interactable?.Interact(this);
                return true;
            }
            return false;
        }

        public virtual IEnumerator PlayerAttack(WeaponItem item) {
            _isAttacking = true;
            ShootEvent.Invoke(item);
            Attack(item.GetWeapon.Damage, item.GetWeapon.Range);
            _hand.sprite = item.ActiveSprite;
            _handTicker = 0;
            yield return new WaitForSeconds(item.GetWeapon.AttackSpeed);
            _hand.sprite = item.NormalSprite;
            _isAttacking = false;
        }
    }
}