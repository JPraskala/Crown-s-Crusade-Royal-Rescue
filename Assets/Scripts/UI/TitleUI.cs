using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button creditsButton;

        private void Start()
        {
            if (startButton == null || creditsButton == null)
            {
                Debug.LogError("The buttons in the scene Title are not setup.");
                Application.Quit(1);
            }

            startButton.onClick.AddListener(StartButton);
            creditsButton.onClick.AddListener(CreditsButton);
        }

        private void StartButton()
        {
            UIManager.Instance.ExecuteButton(startButton);
        }

        private void CreditsButton()
        {
            UIManager.Instance.ExecuteButton(creditsButton);
        }
    }
}