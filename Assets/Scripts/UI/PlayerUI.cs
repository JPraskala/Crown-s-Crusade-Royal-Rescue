using System.Globalization;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
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
            if (healthSlider == null ^ healthSlider.name != "HealthSlider")
            {
                Debug.LogError("The health slider is not setup.");
                Application.Quit(1);
            }

            if (healthCounter == null ^ healthCounter.name != "Health_Counter")
            {
                Debug.LogError("The health counter is not setup.");
                Application.Quit(1);
            }

            if (!TryGetComponent(out m_playerCanvas)) gameObject.AddComponent<CanvasGroup>();

            healthSlider.minValue = 0.0f;
            healthSlider.maxValue = 150.0f;
        }

        private void Update()
        {
            m_playerCanvas.alpha = PlayerManager.Instance.ScriptIsOn() ? 1f : 0f;

            if (m_playerCanvas.alpha == 0f) return;
            
            healthSlider.value = PlayerManager.Instance.GetHealth();
            healthCounter.text = healthSlider.value.ToString(CultureInfo.CurrentCulture);
        }
    }
}
