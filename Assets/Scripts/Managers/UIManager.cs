using UnityEditor;
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
            Title,
            Controls,
            Instructions,
            Exit
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
                    if (PlayerManager.Instance.GetHealth() != 250)
                        PlayerManager.Instance.ResetHealth();
                    if (PlayerManager.Instance.GetPotions() != 0)
                        PlayerManager.Instance.ResetPotions();
                    break;
                case nameof(TransitionTags.Credits):
                    SceneLoader.LoadScene(SceneLoader.MyScenes.Credits);
                    break;
                case nameof(TransitionTags.Title):
                    SceneLoader.LoadScene(SceneLoader.MyScenes.Title);
                    break;
                case nameof(TransitionTags.Controls):
                    SceneLoader.LoadScene(SceneLoader.MyScenes.Controls);
                    break;
                case nameof(TransitionTags.Instructions):
                    SceneLoader.LoadScene(SceneLoader.MyScenes.Instructions);
                    break;
                case nameof(TransitionTags.Exit):
                    #if UNITY_EDITOR
                        EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
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
