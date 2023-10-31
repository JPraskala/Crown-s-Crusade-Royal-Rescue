using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake() 
    {
        if (!instance) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start() 
    {
        SceneLoader.LoadScene("Title");
    }
}