using goofygame.creature.player;
using goofygame.inventory;
using UnityEngine;

namespace goofygame.enviroment.interactable {
    public class Collectable : MonoBehaviour, IInteractable {

        public Item Item;
        GameObject _player;
        [SerializeField] Transform _sprite;

        private void Awake() {
            _player = GameObject.Find("Player");
        }

        private void Start() {
            _sprite.GetComponent<SpriteRenderer>().sprite = Item.NormalSprite;
        }

        private void Update() {
            _rotate();
        }

        private void _rotate() {
            var lookDir = _player.transform.position - transform.position;
            lookDir.y = 0; // keep only the horizontal direction
            _sprite.rotation = Quaternion.LookRotation(lookDir);
        }

        public void Interact(Player player) {
            if(player.inventory.addItem(new ItemStack(Item))) {
                Destroy(gameObject);
            }
            
        }
    }

}
