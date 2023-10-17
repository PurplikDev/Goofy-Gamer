using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using goofygame.creature;
using goofygame.creature.player;

namespace goofygame.enviroment.interactable {
    public class Door : MonoBehaviour, IInteractable {
        [SerializeField] private List<Switch> _switches = new List<Switch>();

        public DoorType doorType = DoorType.SLIDE_VERTICAL;

        private float _ticker = 1f;
        [SerializeField] private float _tickerScaler = 1f;

        public bool IsInteractable = false;
        private bool _isOpen = false;

        [SerializeField] private Transform _movingPart;

        void Update() {
            if(_ticker < 1f)
                _ticker += _tickerScaler * Time.deltaTime;

            switch(doorType) {
                case DoorType.SLIDE_HORIZONTAL:
                    _movingPart.transform.position = new Vector3(
                        Mathf.Lerp(
                            transform.position.x + (_isOpen ? 0 : 4f),
                            transform.position.x + (_isOpen ? 4f : 0),
                            _ticker),
                        _movingPart.transform.position.y,
                        _movingPart.transform.position.z);
                    break;

                case DoorType.SLIDE_VERTICAL:
                    _movingPart.transform.position = new Vector3(
                        _movingPart.transform.position.x,
                        Mathf.Lerp(
                            transform.position.y + (_isOpen ? 0 : 4.5f),
                            transform.position.y + (_isOpen ? 4.5f : 0),
                            _ticker),
                        _movingPart.transform.position.z);
                    break;

                case DoorType.ROTATIONAL:
                    _movingPart.transform.rotation = Quaternion.Slerp(
                        _movingPart.transform.rotation,
                        new Quaternion(
                            transform.rotation.x,
                            transform.rotation.y + (_isOpen ? 1f : 0f),
                            transform.rotation.z,
                            transform.rotation.w
                            ),
                        _ticker
                        );
                    break;
            }
        }

        public void Open() {
            if(!checkSwitches())
                return;

            _isOpen = !_isOpen;
            _ticker = 0;
        }

        private bool checkSwitches() {
            foreach(Switch _switch in _switches) {
                if(!_switch.State) {
                    return false;
                }
            }
            return true;
        }

        public void Interact(Player player) {
            if(IsInteractable)
                Open();
        }

        private void OnTriggerEnter(Collider other) {
            var creature = other.GetComponent<Creature>();
            if(creature != null && !IsInteractable) {
                Open();
            }
        }

        private void OnTriggerExit(Collider other) {
            var creature = other.GetComponent<Creature>();
            if(creature != null && !IsInteractable) {
                Open();
            }
        }
    }

    public enum DoorType {
        SLIDE_HORIZONTAL,
        SLIDE_VERTICAL,
        ROTATIONAL
    }
}