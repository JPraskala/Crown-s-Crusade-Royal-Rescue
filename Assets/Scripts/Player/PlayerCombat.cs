using UnityEngine;
using UnityEngine.InputSystem;
using Managers;
using System.Collections;
using AI.Wizard;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private int m_block;
        private int m_attack;
        private int m_defend;
        private int m_hurt;
        private int m_defeated;
        private Animator m_anim;
        private Game m_game;
        [SerializeField] private GameObject[] enemies;
        private bool m_condition;
        

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
            m_defend = Animator.StringToHash("Player.HeroKnight_Block");
            m_hurt = Animator.StringToHash("Player.HeroKnight_Hurt");
            m_defeated = Animator.StringToHash("Player.Death");
        }
        
        private void PlayerBlock(InputAction.CallbackContext context)
        {
            m_condition = context.started && PlayerMovement.Instance.IsGrounded();
            m_anim.SetBool(m_block, m_condition);
        }

        private void PlayerAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            m_anim.SetTrigger(m_attack);

            foreach (var enemy in enemies)
            {
                var damage = CriticalDamage() ? DamageDealt() * 2 : DamageDealt();
                if (enemy.name == "LightBandit" ^ enemy.name == "HeavyBandit")
                    BanditManager.Instance.HandleBanditHurt(transform, 1.5f, damage);
                else
                {
                    if (SceneLoader.CurrentSceneIndex() == 7)
                        Wizard.Instance.WizardHurt(2.1f, 0.5f, damage);
                }
            }
        }
        
        private static int DamageDealt()
        {
            return Random.Range(20, 51);
        }

        private static bool CriticalDamage()
        {
            return Random.Range(1, 101) <= 5;
        }
        
        public void Blocked()
        {
            m_anim.Play(m_defend);
        }

        public void NotBlocked()
        {
            m_anim.Play(m_hurt);
        }

        public bool IsBlocking()
        {
            return m_condition;
        }

        public void PlayerDeath()
        {
            m_anim.Play(m_defeated);
            StartCoroutine(Delay());
        }

        private static IEnumerator Delay()
        {
            yield return new WaitForSeconds(2.5f);
            SceneLoader.LoadScene(SceneLoader.MyScenes.Defeat);
        }
        
        private void OnDestroy()
        {
            m_game.Player.Block.started -= PlayerBlock;
            m_game.Player.Block.canceled -= PlayerBlock;
            m_game.Player.Fire.performed -= PlayerAttack;
        }
    }
}
