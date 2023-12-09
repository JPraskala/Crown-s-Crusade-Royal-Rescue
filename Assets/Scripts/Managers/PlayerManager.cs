using Player;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private bool m_scriptOn;
        private int m_playerHealth;
        [SerializeField] private GameObject playerPrefab;
        private GameObject m_player;
        private bool m_playerSetup;
        private uint m_potionsAmount;
        
        public static PlayerManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            m_potionsAmount = 0;
            m_playerHealth = 250;
            m_playerSetup = false;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ToggleScript()
        {
            m_scriptOn = SceneLoader.CurrentSceneIndex() >= 6;
            
            gameObject.SetActive(m_scriptOn);

            switch (m_scriptOn)
            {
                case false when m_player != null:
                    Destroy(m_player);
                    m_player = null;
                    m_playerSetup = false;
                    break;
                case true:
                    PlayerInstance();
                    break;
            }
        }
        
        private void PlayerInstance()
        {
            if (m_player != null)
            {
                m_player.transform.position = SetPlayer();
                return;
            }
            
            if (playerPrefab == null ^ playerPrefab.name != "Gareth")
            {
                Debug.LogError("The player prefab is not set to Gareth.");
                Application.Quit(1);
            }
            
            m_player = InstantiatePlayer();
            
            if (m_player == null)
            {
                print("The player is null. Cannot start the game.");
                Application.Quit(1);
            }
            
            
            DontDestroyOnLoad(m_player);
            
            m_player.SetActive(true);
            m_player.name = "Gareth";
            m_player.tag = "Player";
            m_player.layer = 0;
            m_playerSetup = true;
        }

        public bool ScriptIsOn()
        {
            return m_scriptOn;
        }

        public bool PlayerSetup()
        {
            return m_playerSetup;
        }
        
        public Transform GetPlayer()
        {
            return m_player ? m_player.transform : null;
        }

        private GameObject InstantiatePlayer()
        {
            var sceneIndex = SceneLoader.CurrentSceneIndex();

            if (sceneIndex < 6) return null;

            var vector = sceneIndex switch
            {
                6 => new Vector3(-12.13872f, -4.09f, 0.0f),
                7 => new Vector3(-11.76f, -0.112f, 0.0f),
                _ => Vector3.zero
            };

            return Instantiate(playerPrefab, vector, Quaternion.identity);
        }

        private static Vector3 SetPlayer()
        {
            var sceneIndex = SceneLoader.CurrentSceneIndex();

            if (sceneIndex < 6) return Vector3.zero;

            var vector = sceneIndex switch
            {
                6 => new Vector3(-12.13872f, -4.09f, 0.0f),
                7 => new Vector3(-11.76f, -0.112f, 0.0f),
                _ => Vector3.zero
            };

            return vector;
        }
        
        public int GetHealth()
        {
            return m_playerHealth;
        }

        public void PlayerHurt(float amount)
        {
            m_playerHealth -= Mathf.RoundToInt(Mathf.Abs(amount));

            if (m_playerHealth < 0)
                m_playerHealth = 0;
            
            if (m_playerHealth == 0)
                PlayerCombat.Instance.PlayerDeath();
        }

        public void ResetHealth()
        {
            m_playerHealth = 250;
        }

        public uint GetPotions()
        {
            return m_potionsAmount;
        }

        public void PotionCollected()
        {
            m_potionsAmount++;
        }

        private void IncreaseHealth()
        {
            if (m_playerHealth == 250 || m_potionsAmount == 0)
                return;

            var health = m_playerHealth / .25f;
            m_playerHealth = Mathf.RoundToInt(health);
            m_potionsAmount--;

            if (m_playerHealth > 250)
                m_playerHealth = 250;
        }

        public void ResetPotions()
        {
            m_potionsAmount = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                IncreaseHealth();
        }
    }
}
