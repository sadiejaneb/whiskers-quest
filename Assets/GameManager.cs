using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI killText;
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
        UpdateKillText();
    }

    public void GemCollected()
    {
        gemsCollected++;

        if (gemsCollected >= totalGems)
        {
            if (winText != null)
            {
                winText.text = "You Win!";
                winText.enabled = true;
            }
        }
    }
}
