using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace AI.Bandits
{
    public class BanditCombat : MonoBehaviour
    {
        private enum BanditCombatStates
        {
            Attack,
            Hurt,
            Death,
            Recover
        }

        private int m_banditHealth;
        private Animator m_anim;
        private int m_hurtParam;
        private int m_attackParam;
        private int m_defeatedParam;
        private int m_recoverParam;
        private const float TargetFrame = (float)4 / 7;
        private BanditCombatStates m_combatState;
        private bool m_canAttack;
        private bool m_canRecover;
        private BanditMovement m_banditMovement;
        
        #region Setting Up
        
        public BanditCombat Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_banditMovement = GetComponent<BanditMovement>();
            m_anim = GetComponent<Animator>();
            m_hurtParam = Animator.StringToHash("hurt");
            m_attackParam = Animator.StringToHash("Base Layer.Attack");
            m_defeatedParam = Animator.StringToHash("defeated");
            m_recoverParam = Animator.StringToHash("recover");
            m_combatState = BanditCombatStates.Attack;
            m_canAttack = true;
            m_canRecover = true;
            StartCoroutine(WaitFrame());
            m_banditHealth = m_banditMovement.IsHeavyBandit() ? 100 : 50;
        }

        private static IEnumerator WaitFrame()
        {
            yield return new WaitForEndOfFrame();
        }
        #endregion
        
        #region Combat
        private void Update()
        {
            if (!m_banditMovement.InCombatState()) return;
            
            if (m_banditMovement.Distance() > 1.5f)
                m_canAttack = true;

            switch (m_combatState)
            {
                case BanditCombatStates.Attack:
                    if (m_banditMovement.Distance() <= 1.5f && m_canAttack && m_banditHealth > 0)
                    {
                        m_canAttack = false;
                        m_anim.Play(m_attackParam);
                        StartCoroutine(AttackPause());
                    }
                    break;
                case BanditCombatStates.Hurt:
                    m_anim.SetTrigger(m_hurtParam);
                    m_combatState = m_banditHealth > 0 ? BanditCombatStates.Attack : BanditCombatStates.Death;
                    break;
                case BanditCombatStates.Death:
                    m_anim.SetTrigger(m_defeatedParam);
                    break;
                case BanditCombatStates.Recover:
                    m_anim.SetTrigger(m_recoverParam);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
        }
        
        #region Attack State

        private IEnumerator AttackPause()
        {
            yield return new WaitForSecondsRealtime(AttackIntermission());
            m_canAttack = true;
        }

        private static float AttackIntermission()
        {
            return Random.Range(1.4f, 2.8f);
        }
        public int DamageDealt()
        {
            var lowerBounds = m_banditMovement.IsHeavyBandit() ? 25 : 15;
            var upperBounds = m_banditMovement.IsHeavyBandit() ? 41 : 31;

            return Random.Range(lowerBounds, upperBounds);
        }
        #endregion
        
        #region Hurt State
        public void BanditHurt(int amount)
        {
            if (m_banditHealth <= 0)
                return;
            
            m_banditHealth -= amount;
            m_combatState = BanditCombatStates.Hurt;
        }
        #endregion
        #region Extra Methods

        public bool BanditAlive()
        {
            return m_banditHealth > 0;
        }

        public bool AtTargetFrame()
        {
            var banditInfo = m_anim.GetCurrentAnimatorStateInfo(0);
            if (!banditInfo.IsName("Attack"))
                return false;

            const float toleranceLevel = 0.05f;
            var result = Mathf.Abs(banditInfo.normalizedTime - TargetFrame);
            return result < toleranceLevel;
        }
        #endregion
        #endregion
    }
}
