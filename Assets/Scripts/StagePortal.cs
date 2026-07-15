using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StagePortal : MonoBehaviour
{
    public string nextScene = "2.StageSelect";

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
        // 안전장치: fadeImage가 인스펙터에 할당되지 않았다면 경고를 띄우고 바로 씬을 이동시킵니다.
        if (fadeImage == null)
        {
            Debug.LogError("StagePortal: 'fadeImage'가 할당되지 않았습니다! 유니티 인스펙터에서 Canvas 내부의 Image를 드래그 앤 드롭 해주세요.");
            SceneManager.LoadScene(nextScene);
            yield break;
        }

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