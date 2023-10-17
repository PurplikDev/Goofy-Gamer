using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using goofygame.inventory;
using System.Collections;
using System;
using goofygame.inventory.gun;

namespace goofygame.creature.player {
    public class Player : Creature {
        [SerializeField] TextMeshProUGUI _healthDisplay;

        [SerializeField] GameObject _healTimerIndicator;
        Slider _healingIndicatorSlider;

        [SerializeField] Image _hand;

        private Item _activeItem;

        public event Action<int> SwitchItemEvent;

        private bool _isAttacking = false;

        public Inventory inventory = new Inventory();

        private float _healthTimer = 0;

        private void Awake() {
            healEvent += _updateHealth;
            damageEvent += _updateHealth;
            SwitchItemEvent += _switchItem;
            _updateHealth(Health);
            _healingIndicatorSlider = _healTimerIndicator.GetComponentInChildren<Slider>();
        }

        private void _updateHealth(int health) {
            StringBuilder healthText = new StringBuilder("[ Health: ").Append(Health).Append(" ]");
            _healthDisplay.text = healthText.ToString();
        }

        private void Update() {
            if(Input.GetKey(KeyCode.Mouse0)) {
                if(_activeItem is WeaponItem _gunItem) {
                    if(!_isAttacking) {
                        StartCoroutine(playerAttack(_gunItem));
                    }
                }
                if(_activeItem == ItemRegistry.medkid) {
                        _healTimerIndicator.SetActive(true);
                        _healthTimer += 0.5f * Time.deltaTime;
                        _healingIndicatorSlider.value = _healthTimer;
                        if(_healthTimer >= 1.5f) {
                            Heal(5);
                            _healthTimer = 0;
                            inventory.removeItem(ItemRegistry.medkid);
                            SwitchItemEvent.Invoke(0);
                        }
                } else {
                    _healTimerIndicator.SetActive(false);
                    _healthTimer = 0;
                    _healingIndicatorSlider.value = 0;
                }
            }

            if(Input.GetKeyDown(KeyCode.Space)) {
                inventory.addItem(new ItemStack(ItemRegistry.medkid));
            }

            if(Input.GetKeyDown(KeyCode.P)) {
                inventory.addItem(new ItemStack(ItemRegistry.handgun));
            }

            if(Input.GetKeyDown(KeyCode.E)) {
                Interact();
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


        private void _switchItem(int itemIndex) {
            try {
                _activeItem = inventory.Container[itemIndex].Item;
            } catch (ArgumentOutOfRangeException) {
                _activeItem = null;
                _hand.sprite = null;
                _hand.color = new Color(0, 0, 0, 0);
                return;
            }
            _hand.color = Color.white;
            _hand.sprite = _activeItem.NormalSprite;
        }

        public virtual IEnumerator playerAttack(WeaponItem item) {
            _isAttacking = true;
            Attack(item.GetWeapon.Damage, item.GetWeapon.Range);
            _hand.sprite = item.ActiveSprite;
            yield return new WaitForSeconds(item.GetWeapon.AttackSpeed);
            _hand.sprite = item.NormalSprite;
            _isAttacking = false;
        }
    }
}