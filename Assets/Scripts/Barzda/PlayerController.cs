using UnityEngine;

namespace Barzda
{
    public class PlayerController : MonoBehaviour
    {
        private const float DEAD_Y_POS = -30.0f;

        public SmoothCamera smoothCamera;
        public Animator animator;

        public float moveSpeed = 5f;
        public float jumpForce = 10f;
        public Transform groundCheck;
        public LayerMask groundLayer;

        private Rigidbody rb;
        private bool isGrounded;

        private float moveDirection;
        private bool jumpButtonDown;

        private Vector3 startPosition;

        private Camera cachedCamera;

        private void Awake()
        {
            cachedCamera = Camera.main;
            cachedCamera.enabled = false;
            smoothCamera.GetComponent<Camera>().tag = "MainCamera";
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            startPosition = rb.position;
        }

        private void Update()
        {
            // Player movement
            moveDirection = Input.GetAxis("Horizontal");
            jumpButtonDown = Input.GetButtonDown("Jump");

            // Player jump
            if (isGrounded && jumpButtonDown)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
        }

        private void FixedUpdate()
        {

            // Check if the player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);


            Vector3 movement = transform.forward * moveDirection * moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);


            if (!isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 30.0f * Time.deltaTime, rb.velocity.z);
            }

            if (rb.velocity.z > 0)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else if (rb.velocity.z < 0)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, -1.0f);
            }

            smoothCamera.CameraUpdate();

            if (rb.position.y < DEAD_Y_POS)
            {
                rb.position = startPosition;
            }

            bool isRunning = isGrounded && moveDirection != 0.0f;
            bool isJumping = !isGrounded;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle") && !isRunning && !isJumping)
            {
                animator.SetTrigger("idle");
            }
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("run") && isRunning && !isJumping)
            {
                animator.SetTrigger("run");
            }
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("jump") && isJumping)
            {
                animator.SetTrigger("jump");
            }

        }

        private void OnDestroy()
        {
            cachedCamera.enabled = true;
        }
    }
}
