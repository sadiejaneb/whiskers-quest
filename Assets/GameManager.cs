using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text killText;
    public TMP_Text collectionText;
    public Text winText;

    private int killCount = 0;
    private int totalGems = 6;
    private int gemsCollected = 0;

    void Start()
    {
        // Ensure you've assigned the TextMeshPro and UI Text components in the Inspector
        if (killText == null)
        {
            Debug.LogError("TextMeshPro component is not assigned!");
        }

        if (winText != null)
        {
            winText.enabled = false;
        }

        UpdateKillText();
    }

    void UpdateKillText()
    {
        killText.text = "Kills: " + killCount.ToString();
    }

    public void EnemyKilled()
    {
        killCount++;
        Debug.Log(killCount.ToString());
        UpdateKillText();
    }

    public void GemCollected()
    {
        gemsCollected++;
        UpdateCollectionText();

        if (gemsCollected >= totalGems)
        {
            if (winText != null)
            {
                winText.text = "You Win!";
                winText.enabled = true;
            }
        }
    }
    void UpdateCollectionText()
    {
        if (collectionText != null)
        {
            collectionText.text = "Gems: " + gemsCollected;
        }
    }
}
