using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using goofygame.creature.player;

namespace goofygame.inventory {
    public class InventoryDisplay : MonoBehaviour {
        [SerializeField] List<TextMeshProUGUI> slotsDisplay = new List<TextMeshProUGUI>(6);

        [SerializeField] GameObject displayParent;

        [SerializeField] Player player;

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
    }
}