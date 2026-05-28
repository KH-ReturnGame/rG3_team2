using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class StagePortal : MonoBehaviour
{
    public string nextScene = "StageSelect";

    [Header("Fade")]
    public Image fadeImage;

    public float fadeDuration = 1.5f;

    private bool entered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!entered && other.CompareTag("Player"))
        {
            entered = true;
            StartCoroutine(NeonFade());
        }
    }

    IEnumerator NeonFade()
    {
        float time = 0f;

        // 어두운 네온 보라
        Color neonColor = new Color(
            0.12f,
            0f,
            0.22f,
            0f
        );

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(
                0f,
                1f,
                time / fadeDuration
            );

            fadeImage.color = new Color(
                neonColor.r,
                neonColor.g,
                neonColor.b,
                alpha
            );

            yield return null;
        }

        SceneManager.LoadScene(nextScene);
    }
}
