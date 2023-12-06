using Managers;
using UnityEngine;
using TMPro;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthCounter;
        private CanvasGroup m_playerCanvas;
        private static PlayerUI Instance { get; set; }

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

            if (healthCounter == null ^ healthCounter.name != "Health_Counter")
            {
                Debug.LogError("The health counter is not setup.");
                Application.Quit(1);
            }

            if (!TryGetComponent(out m_playerCanvas)) gameObject.AddComponent<CanvasGroup>();

            healthCounter.fontSize = 26.5f;
            healthCounter.autoSizeTextContainer = true;
        }

        private void Update()
        {
            m_playerCanvas.alpha = PlayerManager.Instance.ScriptIsOn() ? 1.0f : 0.0f;

            if ((int)m_playerCanvas.alpha == 0) return;

            healthCounter.text = "Health: " + PlayerManager.Instance.GetHealth();
        }
    }
}
