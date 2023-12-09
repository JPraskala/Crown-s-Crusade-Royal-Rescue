using Managers;
using UnityEngine;
using TMPro;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthCounter;
        private CanvasGroup m_playerCanvas;
        [SerializeField] private TMP_Text banditCounter;
        [SerializeField] private TMP_Text potionsCounter;
        
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

            if ((healthCounter == null ^ healthCounter.name != "Health_Counter") || (banditCounter == null ^ banditCounter.name != "BanditCounter") || (potionsCounter == null ^ potionsCounter.name != "PotionCounter"))
            {
                Debug.LogError("The PlayerUI is not setup.");
                Application.Quit(1);
            }

            if (!TryGetComponent(out m_playerCanvas)) gameObject.AddComponent<CanvasGroup>();

            healthCounter.fontSize = 26.5f;
            healthCounter.alignment = TextAlignmentOptions.Justified;
            banditCounter.color = Color.blue;
        }
        
        private void Update()
        {
            m_playerCanvas.alpha = PlayerManager.Instance.ScriptIsOn() ? 1.0f : 0.0f;

            if ((int)m_playerCanvas.alpha == 0 || SceneLoader.CurrentSceneIndex() < 6) return;

            healthCounter.text = $"HP:{PlayerManager.Instance.GetHealth()}";
            banditCounter.text = $"Bandits Remaining: {BanditManager.Instance.BanditsAlive()}";
            potionsCounter.text = $": {PlayerManager.Instance.GetPotions()}";
        }

        public void ChangeBanditCounterText()
        {
            banditCounter.text = "You have defeated all the bandits. You may now face the boss up ahead.";
        }
    }
}
