using UnityEngine;
using UnityEngine.InputSystem;
using Managers;
using System.Collections;

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
        private GameObject[] m_enemies;
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
            m_enemies = GameObject.FindGameObjectsWithTag("Bandits");
            
            SetupVariables();
        }
        
        private void SetupVariables()
        {
            m_block = Animator.StringToHash("block");
            m_attack = Animator.StringToHash("attack");
            m_defend = Animator.StringToHash("Player.HeroKnight_Block");
            m_hurt = Animator.StringToHash("hurt");
            m_defeated = Animator.StringToHash("defeated");
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

            foreach (var enemy in m_enemies)
            {
                var damage = CriticalDamage() ? DamageDealt() * 2 : DamageDealt();
                BanditManager.Instance.HandleBanditHurt(transform, 1.5f, damage);
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
            print("Blocked");
        }

        public void NotBlocked()
        {
            m_anim.SetTrigger(m_hurt);
            print("Not Blocked");
        }

        public bool IsBlocking()
        {
            return m_condition;
        }

        public void PlayerDeath()
        {
            m_anim.SetTrigger(m_defeated);
            StartCoroutine(Delay());
            SceneLoader.LoadScene(SceneLoader.MyScenes.Defeat);
        }

        private static IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.5f);
        }
    }
}
