using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText; // Reference to the Game Over TextMeshProUGUI
    public SceneFader sceneFader; // Reference to the SceneFader script

    private void Start()
    {
        gameOverText.gameObject.SetActive(false); // Hide the Game Over text at start
    }

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true); // Show the Game Over text
        StartCoroutine(FadeOutAndRestartCoroutine()); // Start the fade out and restart sequence
    }

    private IEnumerator FadeOutAndRestartCoroutine()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before starting the fade out

        sceneFader.FadeAndRestart(); // Call the fade and restart function
    }
}
