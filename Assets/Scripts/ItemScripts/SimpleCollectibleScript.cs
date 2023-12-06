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
    private float initialCountdownValue = 10f;
    private float countdownTimer;

    private PlayerAttackController playerAttackController;

    void Start()
    {
        ResetCountdown();
        playerAttackController = FindObjectOfType<PlayerAttackController>();
    }

    void ResetCountdown()
    {
        countdownTimer = initialCountdownValue;
        if (powerUpText != null)
        {
            powerUpText.text = "Power Up: " + Mathf.CeilToInt(countdownTimer).ToString();
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

        if (!isCounting)
        {
            isCounting = true;
            StartCoroutine(StartCountdown());
        }

        ResetCountdown();
    }

    IEnumerator StartCountdown()
    {
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
        Debug.Log("Countdown timer reached zero!");
        PerformAction();
        Destroy(gameObject);
    }

    void PerformAction()
    {
        playerAttackController.UpdatePlayerDamage(100);
        ResetCountdown();
    }
}
