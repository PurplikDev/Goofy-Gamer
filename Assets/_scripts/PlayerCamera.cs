using UnityEngine;

namespace goofygame.creature.player {
    public class PlayerCamera : MonoBehaviour {
        float sensitivityX, sensitivityY;

        [Range(0f, 500f)]
        public float sensitivity;

        public Transform orientation;
        public Transform head;

        [SerializeField] GameObject player;

        float xRotation, yRotation;

        public bool lockRotation = false;

        public bool isDead = false;

        private float _deathTicker = 0;

        void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            player.GetComponent<Player>().deathEvent += DeathCamera;
        }

        void Update() {
            if (!isDead) {
                NormalRotation();
            } else {
                _deathTicker += Time.deltaTime * 0.125f;

                var rotation = Camera.main.transform.rotation;


                Camera.main.transform.rotation = Quaternion.Slerp(
                    rotation,
                    new Quaternion(0.210207403f, 0.153217167f, -0.234402418f, 0.936692536f),
                    _deathTicker);
            }
            
        }

        private void DeathCamera() {
            isDead = true;
        }

        private void NormalRotation() {
            Camera.main.transform.position = head.position;

            if (lockRotation) {
                return;
            }

            sensitivityX = sensitivity;
            sensitivityY = sensitivity;

            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -80, 80);

            head.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            transform.rotation = Quaternion.Euler(
                xRotation + transform.InverseTransformDirection(player.GetComponent<Rigidbody>().velocity).z * -0.125f,
                yRotation,
                transform.InverseTransformDirection(player.GetComponent<Rigidbody>().velocity).x * -0.5f
                );
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            Camera.main.fieldOfView = 58.71551f + transform.InverseTransformDirection(player.GetComponent<Rigidbody>().velocity).z;
        }
    }
}