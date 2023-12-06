using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Managers;
using Player;

namespace AI.Bandits
{
    public class BanditMovement : MonoBehaviour
    {
        private enum BanditMoveStates
        {
            Idle,
            Move,
            Jump,
            Combat
        }
        
        private NavMeshAgent m_bandit;
        private Rigidbody2D m_rb;
        private Animator m_anim;
        [SerializeField] private LayerMask ground;
        private Transform m_target;
        private bool m_facingRight;
        private bool m_isHeavyBandit;
        private bool m_allowedToMove;
        private bool m_targetLocked;
        private bool m_gravityEnabled;
        private const float Gravity = -9.8f;
        private float m_jumpSpeed;
        private int m_speedParam;
        private int m_jumpParam;
        private int m_movingParam;
        private int m_combatParam;
        private BanditMoveStates m_state;
        private BanditCombat m_combat;
        
        
        #region Setting Up
        public BanditMovement Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (!TryGetComponent(out m_bandit))
                gameObject.AddComponent<NavMeshAgent>();
        
            if (!TryGetComponent(out m_rb))
                gameObject.AddComponent<Rigidbody2D>();
        
            if (!TryGetComponent(out m_anim))
            {
                Debug.LogError("An animator is not attached to the bandits.");
                Application.Quit(1);
            }

            m_combat = GetComponent<BanditCombat>();
            
            SetupVariables();
            SetupNavMesh();
        }
        
        private void SetupVariables()
        {
            m_state = BanditMoveStates.Idle;
            m_facingRight = false;
            m_isHeavyBandit = gameObject.CompareTag("HeavyBandit");
            m_targetLocked = false;
            m_gravityEnabled = true;
            m_jumpSpeed = m_isHeavyBandit ? 6.2f : 6.8f;
            m_speedParam = Animator.StringToHash("speed");
            m_jumpParam = Animator.StringToHash("jump");
            m_movingParam = Animator.StringToHash("isMoving");
            m_combatParam = Animator.StringToHash("inCombat");
        }
        
        private void SetupNavMesh()
        {
            if (!m_bandit.isOnNavMesh)
                Application.Quit(1);
        
            m_bandit.updatePosition = false;
            m_bandit.autoBraking = true;
            m_bandit.autoRepath = false;
            m_bandit.speed = m_isHeavyBandit ? 5.2f : 6.1f;
            m_bandit.acceleration = m_isHeavyBandit ? 4.2f : 4.9f;
            m_bandit.radius = 0.2f;
            m_bandit.stoppingDistance = 0.2f;
        
        }
        #endregion
        #region Bandit States and Animations
        private void Update()
        {
            if (!PlayerManager.Instance.PlayerSetup())
                return;

            if (!m_target)
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                m_target = GameObject.Find("Gareth").transform;
            
            m_allowedToMove = m_state == BanditMoveStates.Jump ^ m_state == BanditMoveStates.Move;

            m_rb.constraints = m_state == BanditMoveStates.Combat
                ? RigidbodyConstraints2D.FreezePosition
                : RigidbodyConstraints2D.FreezePositionY;
            
            m_anim.SetBool(m_movingParam, m_allowedToMove);
        
            switch (m_state)
            {
                case BanditMoveStates.Idle:
                    m_targetLocked = false;
                    m_anim.SetBool(m_combatParam, false);
                    break;
                case BanditMoveStates.Jump:
                    if (!m_bandit.enabled)
                        m_anim.SetTrigger(m_jumpParam);
                    break;
                case BanditMoveStates.Move:
                    if (IsGrounded())
                        m_anim.SetFloat(m_speedParam, m_rb.velocity.x);
                    break;
                case BanditMoveStates.Combat:
                    m_anim.SetBool(m_combatParam, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
        }
        private bool IsGrounded()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, 0.1f, ground);
        }
        #endregion
        #region Bandit Movement
        private void FixedUpdate()
        {
            m_rb.gravityScale = m_gravityEnabled ? 1.0f : 0.0f;
            if (!IsGrounded() && m_gravityEnabled)
                m_rb.velocity += new Vector2(0.0f, Gravity) * Time.fixedDeltaTime;
        
            if (!m_allowedToMove)
                m_bandit.velocity = Vector3.zero;
            
            if (Distance() >= 5.5f)
                m_state = BanditMoveStates.Idle;
            else if (Distance() > 1.5f && (IsFacingPlayer() || m_targetLocked))
                Movement();
            else
                m_state = BanditMoveStates.Combat;
        }
        
        private bool IsFacingPlayer()
        {
            var agentTransform = transform;
            var toPlayer = agentTransform.position - m_target.position;
            toPlayer.Normalize();
            var dotProduct = Vector2.Dot(agentTransform.right, toPlayer);
            return dotProduct >= 0.999f;
        }
        private void Movement()
        {
            if (!m_combat.BanditAlive())
            {
                m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                return;
            }
            m_targetLocked = true;
            var path = new NavMeshPath();
            if (!(m_bandit.CalculatePath(m_target.position, path) && path.status == NavMeshPathStatus.PathComplete))
                return;

            if ((int)transform.position.y == (int)m_target.position.y)
            {
                m_bandit.destination = m_target.position;
                m_state = BanditMoveStates.Move;
                var velocity = m_bandit.velocity;
                m_rb.velocity = velocity;
            }
            
            
            var xDirection = (int)m_bandit.velocity.x;
            if ((!m_facingRight && xDirection > 0) ^ (m_facingRight && xDirection < 0))
                Flip();
            
            if (Distance() >= 2.1f && (m_target.position.y > transform.position.y) && PlayerMovement.Instance.IsGrounded() && (int)transform.position.y == (int)m_target.position.y)
                Jump();
        }
        private void Jump()
        {
            m_rb.AddForce(transform.up * m_jumpSpeed, ForceMode2D.Force);
            m_gravityEnabled = false;
            m_state = BanditMoveStates.Jump;
            StartCoroutine(Jumping());
        }
        
        private IEnumerator Jumping()
        {
            yield return new WaitForSeconds(1.2f);
            m_gravityEnabled = true;
        }
        private void Flip()
        {
            m_facingRight = !m_facingRight;
            var agentTransform = transform;
            var localScale = agentTransform.localScale;
            localScale.x *= -1.0f;
            agentTransform.localScale = localScale;
        }
        #endregion
        #region Public Methods
        public bool IsHeavyBandit()
        {
            return m_isHeavyBandit;
        }

        public bool InCombatState()
        {
            return m_state == BanditMoveStates.Combat;
        }
        
        public float Distance()
        {
            return m_target ? Vector2.Distance(transform.position, m_target.position) : Mathf.Infinity;
        }
        #endregion
    }
}
