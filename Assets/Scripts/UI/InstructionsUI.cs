using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace UI
{
    public class InstructionsUI : MonoBehaviour
    {
        [SerializeField] private Button titleButton;

        private void Start()
        {
            if (titleButton != null)
            {
                titleButton.onClick.AddListener(Title);
                return;
            }
            
            Debug.LogError("The title button in the scene Instructions is not setup.");
        }

        private void Title()
        {
            if (UIManager.ButtonValid(titleButton))
                UIManager.Instance.ExecuteButton(titleButton);
        }
    }
}
