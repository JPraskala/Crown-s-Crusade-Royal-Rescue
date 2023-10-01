using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    CircleCollider2D circle;

    void Start() 
    {
        if (!TryGetComponent<Rigidbody2D>(out rb) || !TryGetComponent<CircleCollider2D>(out circle)) 
        {
            print("Components were not found.");
            Application.Quit(1);
        }

        player = GameObject.Find("Test_Player");
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.name == "Test_Player") 
        {
            gameObject.SetActive(false);
            print("Destroyed");
        }
    }
}
