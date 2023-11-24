using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        private Transform m_player;
        [SerializeField] private Vector3 offset;
        private Camera m_camera;
        private float m_smoothSpeed;
        private float m_padding;

        private static CameraManager Instance { get; set; }

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
            //if (!offset.Equals(new Vector3(-2.2f, 2f, -6f))) offset = new Vector3(-2.2f, 2f, -6f);

            m_camera = Camera.main;

            if (m_camera == null || !m_camera.orthographic || m_camera.name != "Main Camera")
            {
                print("The camera is not properly setup.");
                Application.Quit(1);
            }

            m_padding = 1.0f;
            m_smoothSpeed = 0.45f;
        }

        private static Transform FindPlayer()
        {
            return PlayerManager.Instance.GetPlayer();
        }
        
        private void LateUpdate()
        {
            if (!PlayerManager.Instance.ScriptIsOn()) return;

            m_player = FindPlayer();

            if (!m_player)
            {
                print("m_player is null.");
                Application.Quit(1);
            }
            
            var cameraHeight = 2.0f * m_camera.orthographicSize;
            var cameraWidth = cameraHeight * m_camera.aspect;

            var cameraPosition = m_camera.transform.position;

            var minX = cameraPosition.x - cameraWidth / 2.0f + m_padding;
            var maxX = cameraPosition.x + cameraWidth / 2.0f - m_padding;

            var minY = cameraPosition.y - cameraHeight / 2.0f + m_padding;
            var maxY = cameraPosition.y + cameraHeight / 2.0f - m_padding;

            var targetPosition = m_player.position;

            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

            transform.position = Vector3.Lerp(transform.position, targetPosition + offset, m_smoothSpeed);
        }
    }
}
