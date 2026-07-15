using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FuckAlpha : MonoBehaviour
{
    Image GoatImage; // 내 여친임
    void Start()
    {
        GoatImage = GetComponent<Image>();
        Color newColor = GoatImage.color;
        newColor.a = 0;
        GoatImage.color = newColor;
        StartCoroutine(YamadaCoroutine());
    }

    IEnumerator YamadaCoroutine()
    {
        yield return new WaitForSeconds(2f);
        for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime / 10f)
        {
            Color newColor = GoatImage.color;
            newColor.a = alpha;
            GoatImage.color = newColor;
            yield return null;
        }
        yield return null;
    }
}
