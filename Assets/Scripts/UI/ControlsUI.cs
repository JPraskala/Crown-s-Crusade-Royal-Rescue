using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ControlsUI : MonoBehaviour
    {
        [SerializeField] private Button titleButton;

        private void Start()
        {
            if (titleButton != null)
            {
                titleButton.onClick.AddListener(TitleButton);
                return;
            }
                
            
            Debug.LogError("The title button in the scenes Controls is not setup.");
        }

        private void TitleButton()
        {
            if (UIManager.ButtonValid(titleButton))
                UIManager.Instance.ExecuteButton(titleButton);
        }
    }
}
