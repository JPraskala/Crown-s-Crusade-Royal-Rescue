using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CreditsUI : MonoBehaviour
    {
        [SerializeField] private Button titleButton;

        private void Start()
        {
            if (titleButton == null)
            {
                Debug.LogError("The button in the scene Credits is not setup.");
                Application.Quit(1);
            }
            
            titleButton.onClick.AddListener(TitleButton);
        }

        private void TitleButton()
        {
            if (UIManager.ButtonValid(titleButton))
                UIManager.Instance.ExecuteButton(titleButton);
        }
    }
}
