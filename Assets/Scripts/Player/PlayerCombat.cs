using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private int m_block;
        private int m_attack;
        private Animator m_anim;
        private Game m_game;
        private bool m_inCombat;
        private float m_playerHealth;
        

        public static PlayerCombat Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            m_anim = GetComponent<Animator>();
            m_game = new Game();
            m_game.Enable();

            m_game.Player.Block.started += PlayerBlock;
            m_game.Player.Block.canceled += PlayerBlock;

            m_game.Player.Fire.performed += PlayerAttack;
            
            SetupVariables();
        }

        private void SetupVariables()
        {
            m_block = Animator.StringToHash("block");
            m_attack = Animator.StringToHash("attack");
            m_inCombat = false;
            m_playerHealth = PlayerManager.Instance.GetHealth();
        }

        private void PlayerBlock(InputAction.CallbackContext context)
        {
            var condition = context.started && PlayerMovement.Instance.IsGrounded() && m_inCombat;
            m_anim.SetBool(m_block, condition);
        }

        private void PlayerAttack(InputAction.CallbackContext context)
        {
            if (!context.performed || !m_inCombat) return;
            
            m_anim.SetTrigger(m_attack);
        }

        public bool InCombat()
        {
            return m_inCombat;
        }
    }
}
