using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using goofygame.creature;

namespace goofygame.enviroment.interactable {
    public class Door : MonoBehaviour, IInteractable {
        [SerializeField] private List<Switch> _switches = new List<Switch>();

        public DoorType doorType = DoorType.SLIDE_VERTICAL;

        private float _ticker = 1f;

        public bool IsInteractable = false;
        [SerializeField] private bool _isOpen = false;

        [SerializeField] private Transform _movingPart;

        void Update() {
            if(_ticker < 1f)
                _ticker += 1f * Time.deltaTime;

            switch(doorType) {
                case DoorType.SLIDE_HORIZONTAL:
                    // do slidy stuff
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
                    // spiiiiiiiiiiiin
                    break;
            }
        }

        public void Open() {
            if(!checkSwitches())
                return;

            _isOpen = !_isOpen;
            _ticker = 0;

            Debug.Log("Nonagon Infinity opens the door!");
        }

        private bool checkSwitches() {
            foreach(Switch _switch in _switches) {
                if(!_switch.State) {
                    return false;
                }
            }
            return true;
        }

        public void Interact() {
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