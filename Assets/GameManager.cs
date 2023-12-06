using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text winText; // Reference to the UI Text object for displaying "You Win!"

    private int totalGems = 6; // Total number of gems in the level
    private int gemsCollected = 0; // Counter for collected gems

    void Start()
    {
        // Initialize the win text to be invisible at the beginning
        if (winText != null)
        {
            winText.enabled = false;
        }
    }

    public void GemCollected()
    {
        gemsCollected++;

        // Check if all gems are collected
        if (gemsCollected >= totalGems)
        {
            // Show "You Win!" message on the screen
            if (winText != null)
            {
                winText.text = "You Win!";
                winText.enabled = true;
            }
        }
    }
}
