using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        private bool m_scriptActive;
        private enum TransitionTags
        {
            Start,
            Credits,
            Title
        }
        
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
            m_scriptActive = SceneLoader.CurrentSceneIndex() < 6;
            gameObject.SetActive(m_scriptActive);
        }
        
        public void ExecuteButton(Button button)
        {
            if (!m_scriptActive) return;

            switch (button.tag)
            {
                case nameof(TransitionTags.Start):
                    SceneLoader.LoadScene(SceneLoader.MyScenes.Level1);
                    if (PlayerManager.Instance.GetHealth() != 150)
                        PlayerManager.Instance.ResetHealth();
                    break;
                case nameof(TransitionTags.Credits):
                    SceneLoader.LoadScene(SceneLoader.MyScenes.Credits);
                    break;
                case nameof(TransitionTags.Title):
                    SceneLoader.LoadScene(SceneLoader.MyScenes.Title);
                    break;
                default:
                    print("Invalid Tag");
                    break;
            }
        }

        public static bool ButtonValid(Button button)
        {
            return button.isActiveAndEnabled;
        }
    }
}
