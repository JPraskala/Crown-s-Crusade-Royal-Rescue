using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        
        public static GameManager Instance { get; private set; }

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

        private void Start() 
        {
            SceneLoader.LoadScene(SceneLoader.MyScenes.Title);
        }
    }
}