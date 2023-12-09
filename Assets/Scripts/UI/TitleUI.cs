using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button instructionsButton;

        private void Start()
        {
            if (startButton == null || creditsButton == null || exitButton == null || controlsButton == null || instructionsButton == null)
            {
                Debug.LogError("The buttons in the scene Title are not setup.");
                Application.Quit(1);
            }

            startButton.onClick.AddListener(StartButton);
            creditsButton.onClick.AddListener(CreditsButton);
            exitButton.onClick.AddListener(ExitButton);
            controlsButton.onClick.AddListener(Controls);
            instructionsButton.onClick.AddListener(Instructions);
        }

        private void StartButton()
        {
            if (UIManager.ButtonValid(startButton))
                UIManager.Instance.ExecuteButton(startButton);
        }

        private void CreditsButton()
        {
            if (UIManager.ButtonValid(creditsButton))
                UIManager.Instance.ExecuteButton(creditsButton);
        }

        private void ExitButton()
        {
            if (UIManager.ButtonValid(exitButton))
                UIManager.Instance.ExecuteButton(exitButton);
        }

        private void Controls()
        {
            if (UIManager.ButtonValid(controlsButton))
                UIManager.Instance.ExecuteButton(controlsButton);
        }

        private void Instructions()
        {
            if (UIManager.ButtonValid(instructionsButton))
                UIManager.Instance.ExecuteButton(instructionsButton);
        }
    }
}
