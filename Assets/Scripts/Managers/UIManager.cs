using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        private bool m_scriptActive;
        
        public static UIManager Instance { get; private set; }

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
        
        public void ToggleScript()
        {
            m_scriptActive = SceneLoader.CurrentScene()[0] != 'L';
            gameObject.SetActive(m_scriptActive);
        }

        public void ExecuteButton(Button button)
        {
            if (!m_scriptActive) return;

            switch (button.tag)
            {
                case "Start":
                    SceneLoader.LoadScene("Level1");
                    break;
                case "Credits":
                    SceneLoader.LoadScene("Credits");
                    break;
                case "Title":
                    SceneLoader.LoadScene("Title");
                    break;
                default:
                    print("Invalid Tag");
                    break;
            }
        }
    }
}
