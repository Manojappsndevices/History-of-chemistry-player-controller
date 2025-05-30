using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private enum MoveDir { None, Up, Down, Right, Left }
    private MoveDir currentDir = MoveDir.None;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // If joystick is disabled, force Idle animation and return
        if (!joystick.gameObject.activeInHierarchy)
        {
            if (currentDir != MoveDir.None)
            {
                animator.Play("Idle");
                spriteRenderer.flipX = false;
                currentDir = MoveDir.None;
            }
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 input = joystick.GetInputDirection();
        rb.linearVelocity = input.normalized * moveSpeed;

        // Determine direction for animation
        MoveDir dir = MoveDir.None;
        if (input.magnitude > 0.1f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                dir = input.x > 0 ? MoveDir.Right : MoveDir.Left;
            else
                dir = input.y > 0 ? MoveDir.Up : MoveDir.Down;
        }

        // Animation control by code
        if (dir != currentDir)
        {
            switch (dir)
            {
                case MoveDir.Up:
                    animator.Play("MoveUp");
                    spriteRenderer.flipX = false;
                    break;
                case MoveDir.Down:
                    animator.Play("MoveDown");
                    spriteRenderer.flipX = false;
                    break;
                case MoveDir.Right:
                    animator.Play("MoveSides");
                    spriteRenderer.flipX = false;
                    break;
                case MoveDir.Left:
                    animator.Play("MoveSides");
                    spriteRenderer.flipX = true;
                    break;
                default:
                    animator.Play("Idle");
                    spriteRenderer.flipX = false;
                    break;
            }
            currentDir = dir;
        }

        // If stopped, play Idle
        if (dir == MoveDir.None && currentDir != MoveDir.None)
        {
            animator.Play("Idle");
            spriteRenderer.flipX = false;
            currentDir = MoveDir.None;
        }
    }
}
