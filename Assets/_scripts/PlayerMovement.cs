using UnityEngine;

namespace goofygame.creature.player {
    public class PlayerMovement : MonoBehaviour {
        [Header("Movement")]
        private float movementSpeed;
        public float sprintSpeedMultiplier, crouchSpeedMultiplier;
        [Space]
        float horizontalInput, verticalInput;
        [Space]
        public float groundDrag;
        [Space]
        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask groundMask;
        private bool grounded;
        [Header("Crouching Stuff")]
        public float crouchYScale;
        private float startYScale;
        private CapsuleCollider hitbox;
        [Space]
        public Transform orientation;
        private new Rigidbody rigidbody;
        [Space]
        public MovementState movementState;

        Vector3 moveDirection;

        private bool lockMovement = false;

        private void Awake() {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.freezeRotation = true;

            hitbox = GetComponentInChildren<CapsuleCollider>();
            startYScale = hitbox.height;

            GetComponent<Player>().deathEvent += Die;
        }

        void Update() {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

            ProcessInputs();
            SpeedCap();
            StateHandler();

            rigidbody.drag = grounded ? groundDrag : 0;
        }

        void FixedUpdate() {

            if (lockMovement) return;

            MovePlayer();
        }

        private void ProcessInputs() {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if(Input.GetKeyDown(PlayerKeybinds.crouch)) {
                hitbox.height = crouchYScale;
            }
            if(Input.GetKeyUp(PlayerKeybinds.crouch)) {
                hitbox.height = startYScale;
            }
        }

        private void MovePlayer() {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rigidbody.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }

        private void SpeedCap() {
            Vector3 flatVelocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

            if(flatVelocity.magnitude > movementSpeed) {
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
                rigidbody.velocity = new Vector3(limitedVelocity.x, rigidbody.velocity.y, limitedVelocity.z);
            }
        }

        private void StateHandler() {
            // SETTING STATE TO SPRINTING
            if(Input.GetKey(PlayerKeybinds.sprint)) {
                movementState = MovementState.SPRINTING;
                movementSpeed = 6 * sprintSpeedMultiplier;
            }

            // SETTING STATE TO CROUCHING
            else if(Input.GetKey(PlayerKeybinds.crouch)) {
                movementState = MovementState.CROUCHING;
                movementSpeed = 6 * crouchSpeedMultiplier;
            }

            // SETTING STATE TO WALKING
            else {
                movementState = MovementState.WALKING;
                movementSpeed = 6;
            }
        }

        private void Die() {
            lockMovement = true;
        }

        public enum MovementState {
            SPRINTING,
            WALKING,
            CROUCHING,
            IDLE
        }
    }

    public static class PlayerKeybinds {
        [Header("Movement Keybinds")]
        public static KeyCode forward = KeyCode.W;
        public static KeyCode backwards = KeyCode.S;
        public static KeyCode left = KeyCode.A;
        public static KeyCode right = KeyCode.D;
        public static KeyCode sprint = KeyCode.LeftShift;
        public static KeyCode crouch = KeyCode.LeftControl;

        [Header("Attack Keybinds")]
        public static KeyCode primaryAction = KeyCode.Mouse0;
        public static KeyCode secondaryAction = KeyCode.Mouse1;

        [Header("Functional Keybinds")]
        public static KeyCode openInventory = KeyCode.E;
        //public static KeyCode switchBackSlot = KeyCode.F;
        //public static KeyCode switchFocus = KeyCode.R;

        [Header("Misc Keybinds")]
        public static KeyCode pauseGame = KeyCode.Escape;
    }
}