using UnityEngine;

namespace Gameplay
{
    public class CharacterController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float speed;
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float groundRadius;
        [SerializeField] private Transform feetPosition;
        [SerializeField] private LayerMask groundMask;

        private Transform _transform;
        private Rigidbody2D rb;
        private Animator animator;

        private bool canControl = true;
        private bool facingRight = true;
        private float horizontalInput;

        private void Awake()
        {
            _transform = transform;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            canControl = true;
        }

        private void OnEnable()
        {
            DisplayTextView.OnTextShow += DisplayTextView_OnTextShow;
            DisplayTextView.OnTextHide += DisplayTextView_OnTextHide;
        }

        private void DisplayTextView_OnTextHide(object sender, System.EventArgs e)
        {
            canControl = true;
        }

        private void OnDisable()
        {
            DisplayTextView.OnTextShow -= DisplayTextView_OnTextShow;
            DisplayTextView.OnTextHide -= DisplayTextView_OnTextHide;
        }

        private void DisplayTextView_OnTextShow(object sender, System.EventArgs e)
        {
            canControl = false;
        }

        private void Update()
        {
            if (!canControl)
            {
                horizontalInput = 0;
                animator.SetBool("run", false);
                return;
            }
            horizontalInput = Input.GetAxisRaw("Horizontal");

            animator.SetBool("run", horizontalInput != 0);

            if(horizontalInput < 0 && facingRight)
            {
                _transform.rotation = Quaternion.Euler(0, 180, 0);
                facingRight = false;
            }
            else if(horizontalInput > 0 && !facingRight)
            {
                _transform.rotation = Quaternion.Euler(0, 0, 0);
                facingRight = true;
            }

            bool isOnGround = CheckGround();

            if(Input.GetButtonDown("Jump") && isOnGround)
            {
                rb.velocity = Vector2.up * jumpForce;
            }

            animator.SetBool("ground", !isOnGround);
        }

        private void FixedUpdate()
        {
            if (!canControl)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }

        private bool CheckGround()
        {
            return Physics2D.OverlapCircle(feetPosition.position, groundRadius, groundMask);
        }
    }
}
