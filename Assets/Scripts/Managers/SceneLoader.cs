using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneLoader : MonoBehaviour
    {
        public enum MyScenes
        {
            Title,
            Pause,
            Win,
            Defeat,
            Credits,
            Controls,
            Level1,
            Level2,
            Level3,
            Level4
        }
        
        private static SceneLoader _sceneInstance;
        private static int _sceneIndex; 

        
        private void Awake() 
        {
            if (_sceneInstance == null) 
            {
                _sceneInstance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
            {
                Destroy(gameObject);
            }
        }
        
//         private static bool SceneExists() 
//         {
// #if UNITY_EDITOR
//             var scenePaths = AssetDatabase.FindAssets("t:Scene", new[] {"Assets/Scenes/Main_Scenes"});
//             return scenePaths.Select(AssetDatabase.GUIDToAssetPath).Any(scene => scene.EndsWith(_sceneReference + ".unity"));
// #else
//         for (int i = 0; i < SceneManager.sceneCount; i++)
//         {
//             Scene scene = SceneManager.GetSceneAt(i);
//             if (scene.name == sceneName)
//             {
//                 return true;
//             }
//         }
//         return false;
// #endif
//         }

        public static void LoadScene(MyScenes sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            _sceneIndex = (int)sceneName; 
           SceneManager.LoadScene(_sceneIndex, mode);
           SceneType();
        }

        private static void SceneType() 
        {
            if (_sceneIndex < 6) 
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else 
            {
                Cursor.visible = false;
            }
            _sceneInstance.StartCoroutine(Loading());
        }

        
        private static IEnumerator Loading()
        {
            var sceneName = SceneManager.GetSceneByBuildIndex(_sceneIndex);

            while (!sceneName.isLoaded)
            {
                yield return new WaitForEndOfFrame();
            }
            
            UIManager.Instance.ToggleScript();
            PlayerManager.Instance.ToggleScript();
        }

        public static int CurrentSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}
