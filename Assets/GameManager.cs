using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI killText;
    private int killCount = 0;

    void Start()
    {
        // Ensure you've assigned the TextMeshPro component in the Inspector
        if (killText == null)
        {
            Debug.LogError("TextMeshPro component is not assigned!");
        }

        UpdateKillText();
    }

    void UpdateKillText()
    {
        // Update the text to display the current kill count
        killText.text = "Kills: " + killCount.ToString();
    }

    // Call this method when an enemy is killed
    public void EnemyKilled()
    {
        killCount++;
        Debug.Log(killCount.ToString());
        UpdateKillText();
    }
}
