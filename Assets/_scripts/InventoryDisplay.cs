using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using goofygame.creature.player;
using System;

namespace goofygame.inventory {
    public class InventoryDisplay : MonoBehaviour {
        [SerializeField] List<TextMeshProUGUI> slotsDisplay = new List<TextMeshProUGUI>(6);

        [SerializeField] TextMeshProUGUI _activeItemDisplay;

        [SerializeField] GameObject displayParent;

        [SerializeField] Player player;

        private void Awake() {
            player.SwitchItemEvent += switchItem;
        }

        private void Update() {

            if(Input.GetKey(KeyCode.Tab)) {
                displayParent.SetActive(true);
                int i = 0;
                for(; i < player.inventory.Container.Count; i++) {
                    slotsDisplay[i].text = player.inventory.Container[i].Item.ItemName;
                }
                for(; i < 6; i++) {
                    slotsDisplay[i].text = "";
                }
            } else {
                displayParent.SetActive(false);
            }
        }

        private void switchItem(int itemIndex) {

            try {
                var tempItem = player.inventory.Container[itemIndex].Item;
            } catch(ArgumentOutOfRangeException) {
                _activeItemDisplay.text = "";
                return;
            }
            _activeItemDisplay.text = player.inventory.Container[itemIndex].Item.ItemName;
        }
    }
}