using System;
using UnityEngine;

namespace goofygame.creature {

    public class Creature : MonoBehaviour, ICreature {
        [SerializeField] private Transform head;

        [SerializeField] private int _health = 10;
        public int Health { get { return _health; } }

        protected event Action<int> healEvent;
        protected event Action<int> damageEvent;

        protected event Action deathEvent;

        public void Heal(int amount = 1) {
            _health += amount;
            healEvent?.Invoke(_health);
        }

        public void Damage(int amount = 1) {
            _health -= amount;
            damageEvent?.Invoke(_health);
            if(_health <= 0) {
                deathEvent?.Invoke();
            }
        }

        public bool Attack() {

            RaycastHit _hit;
            Physics.Raycast(head.position, head.forward, out _hit, 5);

            if(_hit.transform != null) {
                var creature = _hit.transform.gameObject.GetComponent<ICreature>();
                creature?.Damage();
                return true;
            }
            return false;
        }
    }
}