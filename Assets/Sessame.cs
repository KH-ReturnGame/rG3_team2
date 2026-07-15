using UnityEngine;

public class Sessame : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }

    void OpenSesame()
    {
        Destroy(gameObject);
    }
}
