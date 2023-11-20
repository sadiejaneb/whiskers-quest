using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeOutUIImage;
    public float fadeSpeed = 3f;

    private void Start()
    {
        // Ensure the image is fully transparent at start
        fadeOutUIImage.color = new Color(0, 0, 0, 0);
    }

    public void FadeAndRestart()
    {
        StartCoroutine(FadeOutAndRestartCoroutine());
    }

    private IEnumerator FadeOutAndRestartCoroutine()
    {
        // Fade to black
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime / fadeSpeed;
            fadeOutUIImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Wait on the black screen for a few seconds
        yield return new WaitForSeconds(3f); 

        // Restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
