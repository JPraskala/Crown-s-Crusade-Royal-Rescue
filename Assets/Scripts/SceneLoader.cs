using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader sceneInstance;
    static InputManager manager;

    void Awake() 
    {
        if (sceneInstance == null) 
        {
            sceneInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start() 
    {
        manager = InputManager.Instance;
    }

    static bool SceneExists(string sceneName) 
    {
    #if UNITY_EDITOR
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene", new[] {"Assets/Scenes/Main_Scenes"});
        foreach(string scenePath in scenePaths) 
        {
            string scene = AssetDatabase.GUIDToAssetPath(scenePath);

            if (scene.EndsWith(sceneName + ".unity")) 
            {
                return true;
            }
        }
        return false;
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
        if (SceneExists(sceneName)) 
        {
            SceneManager.LoadScene(sceneName);
            SceneType(sceneName);
        }
        else 
        {
            throw new UnityException("Scene does not exist.");
        }
    }

    static void SceneType(string activeScene) 
    {
        char activeSceneChar = activeScene[0];
        if (activeSceneChar != 'L') 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else 
        {
            Cursor.visible = false;
        }

        manager.SwapActionMap(activeSceneChar);
    }

    public static bool IsSceneLevel() 
    {
        char sceneCharacter = SceneManager.GetActiveScene().name[0];

        if (sceneCharacter == 'L') 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
