using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        private void Awake() 
        {
            if (!_instance) 
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
            {
                Destroy(gameObject);
            }
        }

        private void Start() 
        {
            SceneLoader.LoadScene(SceneLoader.MyScenes.Level1);
        }
    }
}