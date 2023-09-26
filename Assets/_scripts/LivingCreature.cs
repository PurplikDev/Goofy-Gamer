using System.Collections;
using goofygame.creature.player;
using UnityEngine;

namespace goofygame.creature {
    public class LivingCreature : Creature {
        [SerializeField] Transform _sprite;
        [SerializeField] Sprite _normalSprite;
        [SerializeField] Sprite _hitSprite;
        [SerializeField] Sprite _deathSprite;
        GameObject _player;
        [SerializeField] private bool _isPlayerNear = false;
        [SerializeField] private float _distanceToPlayer = 0;

        private void Awake() {
            _player = GameObject.Find("Player");
            deathEvent += death;
            damageEvent += onHit;
        }

        private void Update() {
            _rotate();

            if(_isPlayerNear) {
                if(_distanceToPlayer < 2.5f) {
                    StartCoroutine(livingCreatureAttack(1f));
                }
            }
        }

        private void _rotate() {
            var lookDir = _player.transform.position - transform.position;
            lookDir.y = 0; // keep only the horizontal direction
            _sprite.rotation = Quaternion.LookRotation(lookDir);
        }

        private void onHit(int filler) {
            if(Health > 0) {
                StartCoroutine(spriteChange(_sprite.GetComponent<SpriteRenderer>(), _hitSprite, _normalSprite, 0.15f));
            }
        }

        private void death() {
            Debug.Log("I'm dead");
            _sprite.GetComponent<SpriteRenderer>().sprite = _deathSprite;
            deathEvent -= death;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }





        private void OnTriggerStay(Collider other) {
            var player = other.GetComponent<Player>();
            if(player != null) {
                _isPlayerNear = true;
                _distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); ;
            }
        }

        private void OnTriggerExit(Collider other) {
            var player = other.GetComponent<Player>();
            if(player != null) {
                _isPlayerNear = false;
            }
        }



        public virtual IEnumerator livingCreatureAttack(float time) {
            yield return new WaitForSeconds(time);
            if(_distanceToPlayer < 2.5f) {
                Attack();
            }
        }

    }
}