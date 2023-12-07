using UnityEngine;

public class GemCollection : MonoBehaviour
{
    public AudioClip collectSound;
    public GameManager gameManager; // Reference to the GameManager script

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        if (collectSound)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        if (gameManager != null)
        {
            gameManager.GemCollected(); // Inform GameManager that a gem is collected
        }

        Destroy(gameObject);
    }
}
