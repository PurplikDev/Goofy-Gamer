using UnityEngine;
namespace goofygame.enviroment.interactable {
    public class Switch : MonoBehaviour {
        private bool _state = false;
        public bool State {
            get {
                return _state;
            }
        }

        public void Activate() {
            _state = !_state;
        }
    }
}