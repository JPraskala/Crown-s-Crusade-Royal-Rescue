using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerInput input;
    Testing inputActions;
    float speed;
    float jumpSpeed;
    float gravity;
    bool gravityEnabled;
    bool facingRight;
    [SerializeField] LayerMask Ground;

    void Awake()
    {
        try
        {
            rb = GetComponent<Rigidbody2D>();
            input = GetComponent<PlayerInput>();
        }
        catch (MissingComponentException u)
        {
            Debug.LogException(u);
        }
    }

    void Start()
    {
        speed = 8f;
        jumpSpeed = 20f;
        gravity = 9.8f;
        facingRight = true;
        gravityEnabled = true;
        inputActions = new Testing();
        inputActions.Enable();
        inputActions.Player.Jump.performed += Jump;
    }

    void Update()
    {
        Vector2 fall = new(0f, -gravity);

        if (!IsGrounded() && gravityEnabled)
        {
            rb.velocity += fall * Time.deltaTime;
        }

        Move();
    }

    void Move()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        float moveX = moveInput.x * speed;

        if ((facingRight && moveInput.x < 0f) ^ (!facingRight && moveInput.x > 0f))
        {
            Flip();
        }

        rb.velocity = new Vector2(moveX, rb.velocity.y);
    }

    void OnEnable()
    {
        return;
    }

    void OnDisable()
    {
        return;
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            gravityEnabled = false;
            float jumpInfo = context.ReadValue<float>();

            Vector2 jumpMovement = new Vector2(rb.velocity.x, jumpInfo * jumpSpeed);
            
            rb.velocity = jumpMovement;
            StartCoroutine(Fall());
        }
        else 
        {
            return;
        }
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    bool IsGrounded()
    {
        float groundDistance = 0.75f;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.localScale / 2, 0f, Vector2.down, groundDistance, Ground);
        return hit.collider != null;
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(.09f);
        gravityEnabled = true;
    }
}
