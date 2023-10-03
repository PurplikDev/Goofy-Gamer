using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace goofygame.enviroment.interactable {
    public class Door : MonoBehaviour {
        [SerializeField] private List<Switch> _switches = new List<Switch>();

        public DoorType doorType = DoorType.SLIDE_VERTICAL;

        public void Open() {
            if(!checkSwitches())
                return;

            switch(doorType) {
                case DoorType.SLIDE_HORIZONTAL:
                    // do slidy stuff
                    break;

                case DoorType.SLIDE_VERTICAL:
                    // do more slidy stuff, but vertical :o
                    break;

                case DoorType.ROTATIONAL:
                    // spiiiiiiiiiiiin
                    break;
            }
        }

        private bool checkSwitches() {
            foreach(Switch _switch in _switches) {
                if(!_switch.State) {
                    return false;
                }
            }
            return true;
        }
    }

    public enum DoorType {
        SLIDE_HORIZONTAL,
        SLIDE_VERTICAL,
        ROTATIONAL
    }
}