using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Managers;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Components")] 
        private Rigidbody2D m_rb;
        private PlayerInput m_playerInput;
        private Animator m_anim;
        private Game m_game;
        [Header("Movement")] 
        private uint m_speed;
        private const float JumpSpeed = 8.2f;
        private bool m_facingRight;
        private Vector2 m_gravity;
        private bool m_gravityEnabled;
        [Header("Animations")] 
        private int m_jump;
        private int m_fall;
        private int m_speedParam;
        [Header("Extra Fields")] 
        private uint m_ground;
        private bool m_grounded;
        private float m_leftEdge;
        private float m_rightEdge;
        private bool m_onLeftEdge;
        private bool m_onRightEdge;
        private bool m_canMove;
        

        public static PlayerMovement Instance { get; private set; }
        
        #region Setting Up

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            SetupComponents();
            SetupVariables();

            m_game = new Game();
            m_game.Enable();
            m_game.Player.Jump.performed += Jump;
        }

        private void SetupComponents()
        {
            if (!TryGetComponent(out m_rb))
            {
                m_rb = gameObject.AddComponent<Rigidbody2D>();
            }

            if (!TryGetComponent(out m_playerInput) || !TryGetComponent(out m_anim))
            {
                Debug.LogError("The player lacks a PlayerInput Component and an Animator Component.");
                Application.Quit(1);
            }
            
            if (m_playerInput.currentActionMap.name != "Player") m_playerInput.SwitchCurrentActionMap("Player");
            
            if (m_anim.runtimeAnimatorController.name == "Player" && m_playerInput.actions.name == "Game") return;
            
            Debug.LogError("Animator and PlayerInput are not properly setup.");
            Application.Quit(1);
        }

        private void SetupVariables()
        {
            m_facingRight = true;
            m_gravity = new Vector2(0f, -9.8f);
            m_gravityEnabled = true;
            m_canMove = true;

            m_jump = Animator.StringToHash("jump");
            m_speedParam = Animator.StringToHash("speed");
            m_fall = Animator.StringToHash("fall");

            m_ground = 3;
            m_grounded = false;
        }
        #endregion
        
        #region Collision Detection
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == m_ground)
            {
                m_grounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.layer == m_ground)
            {
                m_grounded = false;
            }
        }

        #endregion
        
        #region Movement
        private void Update()
        {
            m_rb.gravityScale = m_gravityEnabled ? 1f : 0f;

            switch (m_grounded)
            {
                case true:
                    m_anim.SetBool(m_fall, false);
                    break;
                case false when m_gravityEnabled:
                    m_rb.velocity += m_gravity * Time.deltaTime;
                    m_anim.SetBool(m_fall, true);
                    break;
                default:
                    return;
            }

            var currentScene = SceneLoader.CurrentSceneIndex();

            switch (currentScene)
            {
                case 5:
                    m_leftEdge = -16.47441f;
                    m_rightEdge = 110.4864f;
                    break;
                case 6:
                    m_leftEdge = -13.27f;
                    m_rightEdge = 113.45f;
                    break;
                case 7:
                    break;
            }

            var playerTransform = transform.position;
            m_onLeftEdge = playerTransform.x <= m_leftEdge;
            m_onRightEdge = playerTransform.x >= m_rightEdge;
            
            
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (!m_grounded || !context.performed) return;
            
            m_gravityEnabled = false;
            var jumpInfo = context.ReadValue<float>();
            var jumpMovement = new Vector2(m_rb.velocity.x, jumpInfo * JumpSpeed);
            m_rb.velocity = jumpMovement;
            m_anim.SetTrigger(m_jump);
            StartCoroutine(JumpDuration());
        }

        private IEnumerator JumpDuration()
        {
            yield return new WaitForSeconds(.2f);
            m_gravityEnabled = true;
        }

        private void Move()
        {
            var moveInput = m_game.Player.Move.ReadValue<Vector2>();

            if (m_onLeftEdge ^ m_onRightEdge) m_canMove = false;
            
            var direction = (int)moveInput.x;
            
            if ((direction > 0 && m_onLeftEdge) ^ (direction < 0 && m_onRightEdge)) m_canMove = true;
            
            var moveX = m_speed * moveInput.x;
            
            if ((m_facingRight && direction < 0) ^ (!m_facingRight && direction > 0))
                Flip();
            
            m_rb.velocity = new Vector2(moveX, m_rb.velocity.y);

            if (m_grounded)
                m_anim.SetFloat(m_speedParam, moveX);
            
            
        }

        private void FixedUpdate()
        {
            Move();
            DetermineSpeed();
        }

        private void DetermineSpeed()
        {
            m_speed = (uint)(!m_canMove ? 0 : 6);
        }
        private void Flip()
        {
            m_facingRight = !m_facingRight;
            var playerTransform = transform;
            var localScale = playerTransform.localScale;
            localScale.x *= -1f;
            playerTransform.localScale = localScale;
        }

        public bool IsGrounded()
        {
            return m_grounded;
        }
        #endregion
    }
}
