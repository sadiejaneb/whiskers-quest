using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour
{
    public enum CollectibleTypes { NoType, Type1, Type2, Type3, Type4, Type5 };

    public CollectibleTypes CollectibleType;
    public bool rotate;
    public float rotationSpeed;
    public AudioClip collectSound;
    public GameObject collectEffect;
    public Text powerUpText;

    private bool isCounting = false;

    void Start()
    {
        if (powerUpText != null)
        {
            powerUpText.text = "Power Up: 10";
        }
    }

    void Update()
    {
        if (rotate)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (collectSound)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        if (collectEffect)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        Debug.Log("Collectible collected: " + CollectibleType.ToString());

        // Check if it's Type1 and the countdown is not already active
        if (CollectibleType == CollectibleTypes.Type1 && !isCounting)
        {
            StartCoroutine(StartCountdown());
        }
        else if (CollectibleType == CollectibleTypes.Type1 && isCounting)
        {
            // If another Type1 is collected while counting, 
            // you can add any specific behavior here if needed
        }
        else
        {
            Destroy(gameObject); // Instantly destroy for other types
        }
    }

    IEnumerator StartCountdown()
    {
        isCounting = true;
        float countdownTimer = 10f;

        while (countdownTimer > 0)
        {
            yield return new WaitForSeconds(1.0f);
            countdownTimer--;

            if (powerUpText != null)
            {
                powerUpText.text = "Power Up: " + Mathf.CeilToInt(countdownTimer).ToString();
            }
        }

        isCounting = false;
        Debug.Log("Type1 countdown timer reached zero!");
        PerformType1Action();
        Destroy(gameObject); // Destroy after countdown for Type1
    }

    void PerformType1Action()
    {
        // Implement the action to be performed when the Type1 countdown reaches zero
        // For example:
        Debug.Log("Performing Type1 action!");
        // Your logic here...
    }
}
