using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AudioClip titleAudio;
        [SerializeField] private AudioClip levelAudio;
        [SerializeField] private AudioClip winAudio;
        [SerializeField] private AudioClip loseAudio;
        private AudioSource m_audioSource;
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
            m_audioSource = GetComponent<AudioSource>();
            SceneLoader.LoadScene(SceneLoader.MyScenes.Title);
        }

        public void SetAudio()
        {
            switch (SceneLoader.CurrentSceneIndex())
            {
                case 0:
                    m_audioSource.clip = titleAudio;
                    m_audioSource.loop = false;
                    m_audioSource.volume = 0.7f;
                    break;
                case 2:
                    m_audioSource.clip = winAudio;
                    m_audioSource.loop = false;
                    m_audioSource.volume = 0.6f;
                    break;
                case 3:
                    m_audioSource.clip = loseAudio;
                    m_audioSource.loop = false;
                    m_audioSource.volume = 0.6f;
                    break;
                case 6:
                case 7:    
                    m_audioSource.clip = levelAudio;
                    m_audioSource.loop = true;
                    m_audioSource.volume = 0.2f;
                    break;
                default:
                    m_audioSource.clip = null;
                    break;
            }
            
            if (m_audioSource.clip)
                m_audioSource.Play();
        }
    }
}