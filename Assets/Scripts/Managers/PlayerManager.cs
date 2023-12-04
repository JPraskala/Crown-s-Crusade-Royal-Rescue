using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private bool m_scriptOn;
        private int m_playerHealth;
        [SerializeField] private GameObject playerPrefab;
        private GameObject m_player;
        
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
            m_playerHealth = 150;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ToggleScript()
        {
            m_scriptOn = SceneLoader.CurrentSceneIndex() >= 5;
            
            gameObject.SetActive(m_scriptOn);

            switch (m_scriptOn)
            {
                case false when m_player != null:
                    Destroy(m_player);
                    m_player = null;
                    break;
                case true:
                    PlayerInstance();
                    break;
            }
        }
        
        private void PlayerInstance()
        {
            if (m_player != null) return;
            
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
        }

        public bool ScriptIsOn()
        {
            return m_scriptOn;
        }

        public Transform GetPlayer()
        {
            return m_player ? m_player.transform : null;
        }

        private GameObject InstantiatePlayer()
        {
            var sceneIndex = SceneLoader.CurrentSceneIndex();

            if (sceneIndex < 5) return null;

            Vector3 vector;

            switch (sceneIndex)
            {
                case 5:
                    vector = new Vector3(-12.13872f, -4.09f, 0f);
                    break;
                default:
                    vector = Vector3.zero;
                    break;
            }

            return Instantiate(playerPrefab, vector, Quaternion.identity);
        }

        public int GetHealth()
        {
            return m_playerHealth;
        }

        public void ChangeHealth(float amount)
        {
            m_playerHealth += Mathf.RoundToInt(amount);

            if (m_playerHealth < 0) m_playerHealth = 0;

            if (m_playerHealth > 150) m_playerHealth = 150;
            
            
        }
    }
}
