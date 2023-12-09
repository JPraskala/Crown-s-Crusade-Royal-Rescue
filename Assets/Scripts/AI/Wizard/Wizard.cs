using System;
using UnityEngine;
using System.Collections;
using Managers;
using Player;
using Random = UnityEngine.Random;

namespace AI.Wizard
{
    public class Wizard : MonoBehaviour
    {
        private Transform m_player;
        private const int MaxHealth = 180;
        private int m_wizardHealth;
        private Animator m_anim;
        private int m_hurt;
        private int m_death;
        private int m_inCombat;
        private int m_idle;
        private bool m_canAttack;
        
        public static Wizard Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_canAttack = false;
            try
            {
                m_player = GameObject.Find("Gareth").transform;
            }
            catch (NullReferenceException)
            {
                print("Caught NullReference Exception.");
            }
            
            m_wizardHealth = MaxHealth;
            m_anim = GetComponent<Animator>();
            m_death = Animator.StringToHash("Base Layer.WizardDeath");
            m_hurt = Animator.StringToHash("Base Layer.WizardHit");
            m_inCombat = Animator.StringToHash("Base Layer.WizardAttack");
            m_idle = Animator.StringToHash("Base Layer.WizardIdle");
        }

        private float DotProduct()
        {
            var wizardTransform = transform;
            var toPlayer = m_player.position - wizardTransform.position;
            toPlayer.Normalize();
            return Vector2.Dot(wizardTransform.right, toPlayer);
        }

        private float Distance()
        {
            return m_player ? Vector2.Distance(transform.position, m_player.position) : Mathf.Infinity;
        } 

        public void WizardHurt(float radius, float dotValue, int damage)
        {
            if (!(Distance() <= radius && DotProduct() >= dotValue))
                return;

            m_wizardHealth -= Mathf.Abs(damage);
            m_anim.Play(m_hurt);
        }

        private void WizardDeath()
        {
            try
            {
                m_anim.Play(m_death);
                StartCoroutine(Delay());
            }
            catch (NullReferenceException)
            {
                print("Caught NullReference Exception");
            }
            
        }

        private static IEnumerator Delay()
        {
            yield return new WaitForSeconds(2.5f);
            SceneLoader.LoadScene(SceneLoader.MyScenes.Win);
        }

        private void Update()
        {
            if (m_wizardHealth <= 0)
            {
                WizardDeath();
                return;
            }

            m_anim.Play(m_canAttack ? m_inCombat : m_idle);

            var wizardInfo = m_anim.GetCurrentAnimatorStateInfo(0);

            if (wizardInfo.IsName("WizardDeath") ^ wizardInfo.IsName("WizardHit"))
                return;

            if (Distance() <= 2.1f && DotProduct() >= 0.5f)
                WizardAttack();
            else
                m_canAttack = false;
            
            if (Distance() <= 2.1f && DotProduct() <= 0.5f)
                Flip();
        }

        private void WizardAttack()
        {
            m_canAttack = true;
            var damage = DamageDealt();

            const float quarterHealth = MaxHealth * .25f;
            if (m_wizardHealth == (int)quarterHealth)
                damage *= 2.0f;
            
            PlayerManager.Instance.PlayerHurt(PlayerCombat.Instance.IsBlocking() ? damage / 2.0f: damage);
            if ((int)m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime == 1)
                StartCoroutine(Wait());
        }

        private static float DamageDealt()
        {
            return Random.Range(.1f, .6f);
        }

        private IEnumerator Wait()
        {
            m_canAttack = false;
            yield return new WaitForSeconds(5.4f);
            m_canAttack = true;
        }

        private void Flip()
        {
            var transform1 = transform;
            var scale = transform1.localScale;
            scale.x = -scale.x;
            transform1.localScale = scale;
        }
    }
}
