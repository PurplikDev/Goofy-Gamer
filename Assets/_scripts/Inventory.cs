using System.Collections.Generic;
using UnityEngine;
using goofygame.inventory.gun;

namespace goofygame.inventory {

    public class Inventory {
        List<ItemStack> inventory = new List<ItemStack>(6);
        public List<ItemStack> Container {
            get {
                return inventory;
            }
        }

        public bool hasItem(Item item) {
            foreach(ItemStack stack in inventory) {
                if(stack.Item == item) {
                    return true;
                }
            }
            return false;
        }

        public bool addItem(ItemStack stack) {
            if(inventory.Count < 6) {
                inventory.Add(stack);
                return true;
            }
            return false;
        }

        public void removeItem(Item item) {
            if(hasItem(item)) {
                foreach(ItemStack stack in inventory) {
                    if(stack.Item == item) {
                        inventory.Remove(stack);
                        return;
                    }
                }
            }
        }
    }

    public class Item {
        string _itemName;
        public string ItemName {
            get {
                return _itemName;
            }
        }

        public Item(string itemName) {
            _itemName = itemName;
        }

        public Sprite ActiveSprite {
            get {
                return Resources.Load<Sprite>("sprites/items/" + _itemName.ToLower() + "_active");
            }
        }

        public Sprite NormalSprite {
            get {
                return Resources.Load<Sprite>("sprites/items/" + _itemName.ToLower() + "_normal");
            }
        }
    }

    public class ItemStack {
        Item _item;
        public Item Item {
            get {
                return _item;
            }
        }

        public ItemStack(Item item) {
            _item = item;
        }
    }

    public static class ItemRegistry {
        public static Item air = new Item("");
        public static Item medkid = new Item("Medkit");
        public static WeaponItem handgun = new WeaponItem("Handgun", 1, 10f, 0.25f);
        public static WeaponItem theBigBaller = new WeaponItem("TheBigBaller", 5, 50f, 2.5f);
    }
}