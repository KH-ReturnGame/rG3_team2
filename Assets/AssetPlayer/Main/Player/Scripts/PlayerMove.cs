using UnityEngine;
using System.Collections;
public class PlayerMove : MonoBehaviour
{
    // 플레이어의 Rigidbody2D를 저장할 변수
    // 매 프레임마다 GetComponent를 호출하지 않기 위해 저장해 둔다.
    private Rigidbody2D rb;

    // 플레이어의 이동 속도
    // Inspector에서 자유롭게 변경할 수 있도록 SerializeField 사용
    [SerializeField]
    private float moveSpeed = 5f;

    // 좌우 입력값을 저장
    // 왼쪽 : -1
    // 입력 없음 : 0
    // 오른쪽 : 1
    private float moveInput;
    // Animator 컴포넌트를 저장할 변수
    private Animator animator;
    // 게임 시작 시 한 번만 실행


    // 점프 힘
    [SerializeField]
    private float jumpForce = 10f;

    // Ground 판정을 할 레이어
    [SerializeField]
    private LayerMask groundLayer;

    // 플레이어 중심에서 GroundCheck 원을 얼마나 아래로 내릴지
    [SerializeField]
    private Vector2 groundCheckOffset = new Vector2(0f, -0.6f);

    // GroundCheck 원의 반지름
    [SerializeField]
    private float groundCheckRadius = 0.2f;

    // 현재 바닥에 있는지 저장
    private bool isGrounded;

    // 대시 속도
    [SerializeField]
    private float dashSpeed = 12f;

    // 대시 지속 시간
    [SerializeField]
    private float dashDuration = 0.2f;

    // 현재 대시 중인지
    private bool isDashing;
    // 대시 쿨타임
    [SerializeField]
    private float dashCooldown = 3f;

    // 현재 대시 가능 여부
    private bool canDash = true;

    private void Awake()
    {
        // 같은 오브젝트에 있는 Rigidbody2D를 가져온다.
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

    }
    // 입력(Input)은 Update에서 받는 것이 가장 적절하다.
    private void Update()
    {
        // Horizontal 입력을 받아온다.
        // A 또는 ← : -1
        // D 또는 → : 1
        moveInput = Input.GetAxisRaw("Horizontal");

        // 왼쪽 입력은 무시
        if (moveInput < 0)
        {
            moveInput = 0;
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        // 플레이어 발밑에 원을 만들어 Ground와 겹치는지 검사
        isGrounded = Physics2D.OverlapCircle(
            (Vector2)transform.position + groundCheckOffset,
            groundCheckRadius,
            groundLayer
        );
        // Animator에 현재 상태 전달
        animator.SetBool("IsGrounded", isGrounded);
        // 스페이스를 눌렀고 바닥에 있을 때만 점프
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) &&
    !isDashing &&
    canDash &&
    moveInput > 0)
        {
            StartCoroutine(Dash());
        }
    }

    // FixedUpdate는 일정한 시간 간격으로 실행된다.
    // Rigidbody를 이용한 물리 이동은 여기서 처리하는 것이 좋다.
    private void FixedUpdate()
    {
        // 대시 중이 아닐 때만 일반 이동
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    // 플레이어를 앞으로 빠르게 이동시키는 대시 함수
    private IEnumerator Dash()
    {
    
        // 대시 시작
        isDashing = true;
        canDash = false;
        // 대시 애니메이션 실행
        animator.SetTrigger("Dash");
        // 앞으로 빠르게 이동
        rb.linearVelocity = new Vector2(dashSpeed, rb.linearVelocity.y);

        // 대시 시간만큼 유지
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        // 대시 종료
        canDash = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            (Vector2)transform.position + groundCheckOffset,
            groundCheckRadius
        );
    }


}