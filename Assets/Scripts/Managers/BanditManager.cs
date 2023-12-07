using System.Linq;
using AI.Bandits;
using UnityEngine;

namespace Managers
{
    public class BanditManager : MonoBehaviour
    {
        private BanditCombat[] m_banditCombats;
        private BanditMovement[] m_banditMovements;
        private Animator[] m_animators;
        
        public static BanditManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_banditCombats = GetComponentsInChildren<BanditCombat>();
            m_banditMovements = GetComponentsInChildren<BanditMovement>();
            m_animators = GetComponentsInChildren<Animator>();

            if (m_banditCombats != null && m_banditMovements != null && m_animators != null)
                return;
            
            Debug.LogError("The BanditManager didn't successfully get all the components.");
            Application.Quit(1);
        }
        
        public void HandleBanditHurt(Transform player, float radius, int damage)
        {
            foreach (var combat in m_banditCombats)
            {
                var distance = Vector2.Distance(player.position, combat.transform.position);
                
                if (distance <= radius)
                    combat.BanditHurt(damage);
            }
        }


        public int BanditsAlive()
        {
            return m_banditCombats.Sum(combat => combat.BanditAlive() ? 1 : 0);
        }
        
        
    }
}
