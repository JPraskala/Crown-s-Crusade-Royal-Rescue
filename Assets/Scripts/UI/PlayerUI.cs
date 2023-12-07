using Managers;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthCounter;
        private CanvasGroup m_playerCanvas;
        [SerializeField] private Image failurePanel;
        [SerializeField] private Button failureButton;
        public static PlayerUI Instance { get; private set; }

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

            if (failureButton == null)
            {
                Debug.LogError("Button failureButton is not setup.");
                Application.Quit(1);
            }

            healthCounter.fontSize = 26.5f;
            healthCounter.alignment = TextAlignmentOptions.Justified;
            if (failurePanel == null)
                gameObject.AddComponent<Image>();
            
            failureButton.onClick.AddListener(ReturnToScene);
            failurePanel.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            m_playerCanvas.alpha = PlayerManager.Instance.ScriptIsOn() ? 1.0f : 0.0f;

            if ((int)m_playerCanvas.alpha == 0) return;

            healthCounter.text = $"HP:{PlayerManager.Instance.GetHealth()}";
        }

        public void FreezeScene()
        {
            failurePanel.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void ReturnToScene()
        {
            if (!failureButton.isActiveAndEnabled)
                return;
            
            
            failurePanel.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            Cursor.visible = false;
        }
    }
}
