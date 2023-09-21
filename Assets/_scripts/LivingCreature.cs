using UnityEngine;

namespace goofygame.creature {
    public class LivingCreature : Creature {
        [SerializeField] Transform _sprite;
        [SerializeField] Sprite _normalSprite;
        [SerializeField] Sprite _deathSprite;
        GameObject _player;
        bool isDead;

        private void Awake() {
            _player = GameObject.Find("Player");
            deathEvent += death;
        }

        private void Update() {
            _rotate();
        }

        private void _rotate() {
            var lookDir = _player.transform.position - transform.position;
            lookDir.y = 0; // keep only the horizontal direction
            _sprite.rotation = Quaternion.LookRotation(lookDir);
        }

        private void death() {
            Debug.Log("I'm dead");
            _sprite.GetComponent<SpriteRenderer>().sprite = _deathSprite;
        }
    }
}