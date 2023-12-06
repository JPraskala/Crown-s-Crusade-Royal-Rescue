using UnityEngine;
using UnityEngine.InputSystem;
using AI.Bandits;
using Unity.VisualScripting;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private int m_block;
        private int m_attack;
        private Animator m_anim;
        private Game m_game;
        private BanditCombat m_combat;
        private BanditMovement m_banditMovement;
        private GameObject[] m_lightBandits;
        

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
            m_lightBandits = GameObject.FindGameObjectsWithTag("LightBandit");

            m_combat = FindObjectOfType<BanditCombat>().Instance;
            m_banditMovement = FindObjectOfType<BanditMovement>().Instance;

            SetupVariables();
        }

        private void Update()
        {
            
            print("Bandit Movement: " + m_banditMovement);
            print("Bandit Combat: " + m_combat);
        }

        private void SetupVariables()
        {
            m_block = Animator.StringToHash("block");
            m_attack = Animator.StringToHash("attack");
        }
        
        private void PlayerBlock(InputAction.CallbackContext context)
        {
            var condition = context.started && PlayerMovement.Instance.IsGrounded();
            m_anim.SetBool(m_block, condition);
        }

        private void PlayerAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            m_anim.SetTrigger(m_attack);
            
            
            if (m_banditMovement.Distance() <= 1.5f)
                m_combat.BanditHurt(DamageDealt());
        }
        
        private static int DamageDealt()
        {
            return Random.Range(20, 51);
        }
    }
}
