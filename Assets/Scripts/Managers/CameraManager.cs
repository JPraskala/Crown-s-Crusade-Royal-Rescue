using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        private Transform m_player;
        [SerializeField] private Vector3 offset;
        private Camera m_camera;
        private float m_smoothSpeed;

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

            m_camera = Camera.main;

            if (m_camera == null || !m_camera.orthographic || m_camera.name != "Main Camera")
            {
                print("The camera is not properly setup.");
                Application.Quit(1);
            }

            m_smoothSpeed = 0.125f;
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

            var targetPosition = m_player.position;
            var cameraPosition = transform.position;

            var desiredPosition = new Vector3(targetPosition.x, targetPosition.y + 2.5f, cameraPosition.z);
            var smoothedPosition = Vector3.Lerp(cameraPosition, desiredPosition, m_smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
