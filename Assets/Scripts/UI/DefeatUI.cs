using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DefeatUI : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button titleButton;

        private void Start()
        {
            if (restartButton != null && titleButton != null)
            {
                restartButton.onClick.AddListener(Restart);
                titleButton.onClick.AddListener(Title);
                return;
            }
                
            
            Debug.LogError("The UI for the scene Defeat is not properly setup.");
            Application.Quit(1);
        }

        private void Restart()
        {
            if (UIManager.ButtonValid(restartButton))
                UIManager.Instance.ExecuteButton(restartButton);
        }

        private void Title()
        {
            if (UIManager.ButtonValid(titleButton))
                UIManager.Instance.ExecuteButton(titleButton);
        }
    }
}
