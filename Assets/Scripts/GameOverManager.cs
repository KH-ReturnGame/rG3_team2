using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("씬 이동 설정")]
    [Tooltip("이동할 게임오버 씬의 이름을 적어주세요.")]
    public string gameOverSceneName = "6.GameOver";

    [Header("암전(Fade Out) 설정")]
    [Tooltip("화면을 덮을 Canvas 안의 UI Image를 넣어주세요.")]
    public Image fadeImage;
    [Tooltip("암전이 진행되는 시간입니다.")]
    public float fadeDuration = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 플레이어가 죽을 때 이 함수가 호출됩니다.
    public void TriggerGameOver()
    {
        Debug.Log("GameOverManager: 암전을 시작합니다.");
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (fadeImage != null)
        {
            float time = 0f;
            Color startColor = new Color(0.12f, 0f, 0.22f, 0f); // 어두운 네온 보라색

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);

                fadeImage.color = new Color(
                    startColor.r,
                    startColor.g,
                    startColor.b,
                    alpha
                );

                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("GameOverManager: fadeImage가 할당되지 않아 즉시 씬을 전환합니다.");
        }

        // 암전이 다 되면 씬을 변경합니다.
        SceneManager.LoadScene(gameOverSceneName);
    }
}