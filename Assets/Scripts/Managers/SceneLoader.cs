using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _sceneInstance;
        private static string _sceneReference;

        
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
        
        private static bool SceneExists() 
        {
#if UNITY_EDITOR
            var scenePaths = AssetDatabase.FindAssets("t:Scene", new[] {"Assets/Scenes/Main_Scenes"});
            return scenePaths.Select(AssetDatabase.GUIDToAssetPath).Any(scene => scene.EndsWith(_sceneReference + ".unity"));
#else
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
#endif
        }

        public static void LoadScene(string sceneName)
        {
            _sceneReference = sceneName;
            if (SceneExists())
            {
                SceneManager.LoadScene(_sceneReference);
                SceneType();
            }
            else
            {
                throw new UnityException("Scene Name " + sceneName + " doesn't exist.");
            }
        }

        private static void SceneType() 
        {
            if (_sceneReference[0] != 'L') 
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
            var sceneName = SceneManager.GetSceneByName(_sceneReference);

            while (!sceneName.isLoaded)
            {
                yield return new WaitForEndOfFrame();
            }
            
            UIManager.Instance.ToggleScript();
            PlayerManager.Instance.ToggleScript();
        }

        public static string CurrentScene()
        {
            return SceneManager.GetActiveScene().name;
        }
        
        
    }
}
