using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using goofygame.inventory;

namespace goofygame.creature.player {
    public class Player : Creature {
        [SerializeField] TextMeshProUGUI _healthDisplay;

        [SerializeField] GameObject _healTimerIndicator;
        Slider _healingIndicatorSlider;

        [SerializeField] Image _hand;
        [SerializeField] Sprite _normalHand;
        [SerializeField] Sprite _handShoot;

        public Inventory inventory = new Inventory();

        private float _healthTimer = 0;

        private void Awake() {
            healEvent += _updateHealth;
            damageEvent += _updateHealth;
            _updateHealth(Health);
            _healingIndicatorSlider = _healTimerIndicator.GetComponentInChildren<Slider>();
        }

        private void _updateHealth(int health) {
            StringBuilder healthText = new StringBuilder("[ Health: ").Append(Health).Append(" ]");
            _healthDisplay.text = healthText.ToString();
        }

        private void Update() {
            if(Input.GetKey(KeyCode.F) && inventory.hasItem(ItemRegistry.medkid)) {
                _healTimerIndicator.SetActive(true);
                _healthTimer += 0.5f * Time.deltaTime;
                _healingIndicatorSlider.value = _healthTimer;
                if(_healthTimer >= 1.5f) {
                    Heal(5);
                    _healthTimer = 0;
                    inventory.removeItem(ItemRegistry.medkid);
                }
            } else {
                _healTimerIndicator.SetActive(false);
                _healthTimer = 0;
                _healingIndicatorSlider.value = 0;
            }

            _hand.sprite = Input.GetKey(KeyCode.Mouse0) ? _handShoot : _normalHand;

            if(Input.GetKeyDown(KeyCode.Mouse0)) {
                Attack();
            }

            if(Input.GetKeyDown(KeyCode.Space)) {
                inventory.addItem(new ItemStack(ItemRegistry.medkid));
            }
        }


    }
}