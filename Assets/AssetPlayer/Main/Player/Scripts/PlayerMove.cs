using UnityEngine;

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

    // 플레이어의 SpriteRenderer를 저장할 변수
    private SpriteRenderer spriteRenderer;
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

    private void Awake()
    {
        // 같은 오브젝트에 있는 Rigidbody2D를 가져온다.
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        // 같은 오브젝트의 SpriteRenderer를 가져온다.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update는 매 프레임 실행된다.
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
        // 스페이스를 눌렀고 바닥에 있을 때만 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // FixedUpdate는 일정한 시간 간격으로 실행된다.
    // Rigidbody를 이용한 물리 이동은 여기서 처리하는 것이 좋다.
    private void FixedUpdate()
    {
        // 현재 y축 속도는 그대로 유지하고
        // x축 속도만 입력값에 따라 변경한다.
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
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