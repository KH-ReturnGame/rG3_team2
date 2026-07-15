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

    [Header("사망 연출 설정 (미래 대비용)")]
    [Tooltip("죽음 애니메이션의 재생 시간을 적어주세요. 애니메이션이 없을 때는 0으로 두면 즉시 삭제됩니다.")]
    public float deathDelay = 0f;

    // 컴포넌트 참조 (있으면 쓰고, 없으면 알아서 패스하도록 예외 처리 완료)
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        currentHp = maxHp;
        Time.timeScale = 1f;

        // 컴포넌트가 있으면 가져오고, 없어도 에러가 나지 않게 감지맨 역할만 합니다.
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHp -= damage;
        Debug.Log("플레이어가 데미지를 입었습니다! 남은 HP: " + currentHp);

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

        // 1. [오류 방지 핵심] 죽는 순간 콜라이더와 물리를 차단합니다.
        // 이 처리가 없으면 죽어서 딜레이되는 도중에 가시에 또 부딪히는 버그가 생깁니다.
        if (col != null) col.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; // 중력 영향 제거 (공중에서 죽었을 때 멈추게 하거나 아래로 안 떨어지게 보호)
        }

        // 2. [미래 대비] Animator 컴포넌트가 플레이어에게 '있을 때만' 재생을 시도합니다.
        // 지금은 Animator가 없어도 에러(NullReferenceException)가 나지 않고 안전하게 넘어갑니다.
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        // 3. 설정한 딜레이 시간만큼 대기합니다. (지금은 0초이므로 바로 통과)
        yield return new WaitForSeconds(deathDelay);

        // 4. 오브젝트 삭제
        Destroy(gameObject);
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}