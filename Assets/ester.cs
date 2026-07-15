using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ester : MonoBehaviour
{
    public GameObject EsterMessage;
    public TextMeshProUGUI EsterMessagetext;
    public string[] messages;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EsterRoutine());
        }
    }

    IEnumerator EsterRoutine()
    {
        EsterMessagetext.text = messages[Random.Range(0, messages.Length)];
        EsterMessage.SetActive(true);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(3.5f);
        EsterMessage.SetActive(false);
        Destroy(gameObject);
    }
}
