using System.Collections;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Transform cameraController;
        [SerializeField] Transform connectionPoint;
        [SerializeField] Transform playerMesh;

        [Header("Configurations")]
        [SerializeField] float rotationSpeed = 7f;
        [SerializeField] float speed = 7.5f;
        [SerializeField] float jumpSpeed = 8.0f;
        [SerializeField] float gravity = 20.0f;

        [Header("Dodge")]
        [SerializeField] float dodgeDistance = 5f;
        [SerializeField] float dodgeDuration = 5f;

        CharacterController controller;
        Vector2 rotation;
        Vector2 movement;
        Vector3 lastMoveDir;
        bool jump;

        bool isLocked = false;
        bool isDodging = false;
        bool canMove = true;
        Vector3 moveDirection;

        public bool IsLocked{ get => isLocked; }
        public bool IsDodging { get => isDodging; }
        public bool CanMove { get => canMove; set => canMove = value; } 
        public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }

        void Start()
        {
            controller = GetComponent<CharacterController>();
            rotation.y = transform.eulerAngles.y;
        }

        #region Input
        public void Movement(Vector2 _movement)
        {
            movement = _movement;
        }
        public void Jump(bool _jump)
        {
            jump = _jump;
        }
        public void Dash()
        {
            if (!isDodging)
            {
                StartCoroutine(DodgePerform());
            }
        }
        #endregion

        void Update()
        {
            if (CanMove)
            {
                if (controller.isGrounded)
                {
                    Vector3 direction = (new Vector3(connectionPoint.position.x, cameraController.position.y, connectionPoint.position.z) - cameraController.position).normalized;

                    Vector3 forward = direction;
                    Debug.DrawRay(transform.position, forward * 2, Color.blue, 0.02f);
                    Vector3 right = cameraController.TransformDirection(Vector3.right);
                    Debug.DrawRay(transform.position, right * 2, Color.red, 0.02f);

                    float curSpeedX = CanMove ? speed * movement.y : 0;
                    float curSpeedY = CanMove ? speed * movement.x : 0;
                    lastMoveDir = moveDirection = (forward * curSpeedX) + (right * curSpeedY);

                    if (jump)
                    {
                        moveDirection.y = jumpSpeed;
                    }
                }

                // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
                // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
                // as an acceleration (ms^-2)
                moveDirection.y -= gravity * Time.deltaTime;

                // Move the controller
                controller.Move(moveDirection * Time.deltaTime);

                // Player Rotation
                if (movement != Vector2.zero)
                {
                    float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraController.eulerAngles.y;
                    Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                    playerMesh.rotation = Quaternion.Lerp(playerMesh.rotation, rotation, Time.deltaTime * rotationSpeed);
                }
            }
        }

        IEnumerator DodgePerform()
        {
            isDodging = true;
            canMove = false;
            float timer = 0f;
            float maxTimer = dodgeDuration;

            CanMove = false;
            while (timer < maxTimer)
            {
                Vector3 dir = lastMoveDir * dodgeDistance * Time.deltaTime;
                controller.Move(dir);
                timer += Time.deltaTime;
                yield return null;
            }
            CanMove = true;
            isDodging = false;
        }
    }
}