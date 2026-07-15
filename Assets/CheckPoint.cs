using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int checkIndex;
    SpriteRenderer spriteRenderer;
    public Sprite OFF_Sprite;
    public Sprite ON_Sprite;
    public GameOverManager gameOverManager;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameOverManager.checkpointIndex = checkIndex;
            spriteRenderer.sprite = ON_Sprite;
        }
    }
}
