using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    bool inLevel;
    Transform target;
    public static CameraManager cameraInstance;
    float smoothSpeed;

    void Awake() 
    {
        if (cameraInstance == null) 
        {
            cameraInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start() 
    {
        if (gameObject.name != "Main Camera") 
        {
            Debug.LogError("Script CameraManager is not attached to Main Camera");
            Application.Quit(1);
        }

        inLevel = SceneLoader.IsSceneLevel();
        smoothSpeed = .2f;
        target = null;
    }

    void LateUpdate() 
    {
        if (!inLevel) 
        {
            target = null;
            return;
        }

        if (target == null) 
        {
            target = FindPlayer();
        }


        if (target != null) 
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, target.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    Transform FindPlayer() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) 
        {
            return player.transform;
        }
        else 
        {
            return null;
        }
    }
}
