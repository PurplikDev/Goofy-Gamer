using System.Text;
using UnityEngine;
using TMPro;

namespace goofygame.creature.player {
    public class Player : Creature {
        [SerializeField] TextMeshProUGUI _healthDisplay;

        private void Awake() {
            healEvent += _updateHealth;
            damageEvent += _updateHealth;
            _updateHealth(Health);
        }

        private void _updateHealth(int health) {
            StringBuilder healthText = new StringBuilder("[ Health: ").Append(Health).Append(" ]");
            _healthDisplay.text = healthText.ToString();
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.D)) {
                Damage();
            }

            if(Input.GetKeyDown(KeyCode.H)) {
                Heal();
            }
        }


    }
}