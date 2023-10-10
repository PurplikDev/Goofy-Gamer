using UnityEngine;
namespace goofygame.enviroment.interactable {
    public class Switch : MonoBehaviour, IInteractable {
        private bool _state = false;
        public bool State {
            get {
                return _state;
            }
        }

        public void Activate() {
            _state = !_state;
            Debug.Log("Switched to: " + _state);
        }

        public void Interact() {
            Activate();
        }
    }
}