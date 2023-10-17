using System.Collections;
using goofygame.creature.player;
using UnityEngine;

namespace goofygame.creature {
    public class LivingCreature : Creature {
        [Space]
        [SerializeField] Transform _sprite;
        [SerializeField] Sprite _normalSprite;
        [SerializeField] Sprite _hitSprite;
        [SerializeField] Sprite _deathSprite;
        [SerializeField] Sprite _attackSprite;
        [Space]
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _attackRange;
        [SerializeField] private int _damage;
        GameObject _player;
        private bool _isPlayerNear = false;
        private float _distanceToPlayer = 0;
        private bool _isAttacking = false;
        private bool _isAlive = true;

        private void Awake() {
            _player = GameObject.Find("Player");
            deathEvent += death;
            damageEvent += onHit;
        }

        private void Update() {
            _rotate();

            if(!_isAlive) { return; }

            if(_isPlayerNear && !_isAttacking) {
                if(_distanceToPlayer < _attackRange) {
                    StartCoroutine(livingCreatureAttack(_attackSpeed));
                }
            }

            if(_distanceToPlayer > _attackRange) {
                _isAttacking = false;
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
            StopAllCoroutines();
            Debug.Log("I'm dead");
            _sprite.GetComponent<SpriteRenderer>().sprite = _deathSprite;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            deathEvent -= death;
            _isAlive = false;
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
            _isAttacking = true;
            yield return new WaitForSeconds(time);
            if(_distanceToPlayer < _attackRange) {
                StartCoroutine(spriteChange(_sprite.GetComponent<SpriteRenderer>(), _attackSprite, _normalSprite, 0.2f));
                Attack(_damage, _attackRange);
                _isAttacking = false;
            }
        }
    }
}