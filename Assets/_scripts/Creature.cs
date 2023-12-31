using System;
using System.Collections;
using UnityEngine;
using goofygame.enviroment.interactable;

namespace goofygame.creature {

    public class Creature : MonoBehaviour, ICreature {
        [SerializeField] protected Transform head;
        [Space]
        [SerializeField] private int _health = 10;
        [SerializeField] protected int _maxHealth = 10;
        public int Health { get { return _health; } }
        protected event Action<int> healEvent;
        protected event Action<int> damageEvent;

        public event Action deathEvent;

        public virtual void Heal(int amount = 1) {
            int value = _health + amount;
            Debug.Log(value);
            _health = value < _maxHealth ? value : _maxHealth;
            healEvent?.Invoke(_health);
        }

        public virtual void Damage(int amount = 1) {
            _health -= amount;
            damageEvent?.Invoke(_health);
            if(_health <= 0) {
                deathEvent?.Invoke();
            }
        }

        public bool Attack(int damage, float range) {
            RaycastHit _hit;
            Physics.Raycast(head.position, head.forward, out _hit, range, -5, QueryTriggerInteraction.Ignore);

            if(_hit.transform != null && !_hit.collider.isTrigger) {
                var creature = _hit.transform.gameObject.GetComponent<ICreature>();
                creature?.Damage(damage);
                return true;
            }
            return false;
        }

        public virtual IEnumerator spriteChange(SpriteRenderer renderer, Sprite sprite1, Sprite sprite2, float time) {
            renderer.sprite = sprite1;
            yield return new WaitForSeconds(time);
            if(_health <= 0) {
                yield break;
            } else {
                renderer.sprite = sprite2;
            }
            
        }
    }
}