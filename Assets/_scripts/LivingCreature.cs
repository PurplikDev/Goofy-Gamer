using UnityEngine;

namespace goofygame.creature {
    public class LivingCreature : Creature {
        [SerializeField] Transform _sprite;
        GameObject _player;

        private void Awake() {
            _player = GameObject.Find("Player");
        }

        private void Update() {
            _rotate();
        }

        private void _rotate() {
            var lookDir = _player.transform.position - transform.position;
            lookDir.y = 0; // keep only the horizontal direction
            _sprite.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}