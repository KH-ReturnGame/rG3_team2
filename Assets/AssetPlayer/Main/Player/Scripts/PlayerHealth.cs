using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("플레이어 체력 설정")]
    public int maxHp = 3;
    private int currentHp;

    [Header("무적 시간 설정")]
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;

    [Header("사망 지연 시간 (연출용)")]
    [Tooltip("사망 애니메이션 재생 시간 설정 (0이면 즉시 숨김)")]
    public float deathDelay = 0f;

    // 컴포넌트 저장용 변수
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer; // 플레이어를 화면에서 숨기기 위한 컴포넌트

    void Start()
    {
        currentHp = maxHp;
        Time.timeScale = 1f;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 플레이어의 스프라이트 렌더러 가져오기
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHp -= damage;
        Debug.Log("플레이어가 데미지를 입었습니다! 현재 HP: " + currentHp);

        if (currentHp <= 0)
        {
            StartCoroutine(DieRoutine());
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private IEnumerator DieRoutine()
    {
        Debug.Log("플레이어 사망 처리 시작");

        // 1. 물리와 충돌을 꺼서 플레이어가 조작되거나 더 맞지 않게 합니다.
        if (col != null) col.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; 
        }

        // 2. 플레이어 조작 스크립트(PlayerMove) 비활성화
        MonoBehaviour moveScript = GetComponent("PlayerMove") as MonoBehaviour;
        if (moveScript != null)
        {
            moveScript.enabled = false;
        }

        // 3. 애니메이션 트리거 실행
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        // 4. 사망 연출 대기 시간 동안 대기
        yield return new WaitForSeconds(deathDelay);

        // 🌟 [핵심]: 플레이어를 Destroy하지 않고 이미지(스프라이트)만 감쪽같이 끕니다!
        // 이 덕분에 자식 카메라가 파괴되지 않고 화면을 계속 그릴 수 있게 됩니다.
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // 5. 싱글톤 매니저에게 게임오버 처리를 부탁합니다.
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.TriggerGameOver();
        }
        else
        {
            Debug.LogWarning("GameOverManager가 씬에 존재하지 않습니다!");
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}