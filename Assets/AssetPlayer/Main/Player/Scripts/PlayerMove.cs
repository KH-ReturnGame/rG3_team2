using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    // 플레이어의 Rigidbody2D를 저장할 변수
    private Rigidbody2D rb;

    // 플레이어의 이동 속도
    [SerializeField]
    public float moveSpeed = 5f;
    
    public bool isSpeedBoosted = false;

    // 좌우 입력값 저장
    private float moveInput;

    // 바라보는 방향
    // 오른쪽 : 1
    // 왼쪽 : -1
    private int facingDirection = 1;

    // Animator 컴포넌트
    private Animator animator;

    // Sprite 좌우 반전
    private SpriteRenderer spriteRenderer;


    // 점프 힘
    [SerializeField]
    private float jumpForce = 10f;

    // Ground 판정 레이어
    [SerializeField]
    private LayerMask groundLayer;

    // GroundCheck 위치
    [SerializeField]
    private Vector2 groundCheckOffset = new Vector2(0f, -0.6f);

    // GroundCheck 반지름
    [SerializeField]
    private float groundCheckRadius = 0.2f;

    // 바닥 여부
    public bool isGrounded;


    // 대시 속도
    [SerializeField]
    private float dashSpeed = 12f;

    // 대시 지속 시간
    [SerializeField]
    private float dashDuration = 0.2f;

    // 대시 중인지
    private bool isDashing;

    // 대시 쿨타임
    [SerializeField]
    private float dashCooldown = 3f;

    // 대시 가능 여부
    private bool canDash = true;
    AudioSource poopSound;
    public AudioClip[] poopSoundClips;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        poopSound = GetComponent<AudioSource>();
    }


    private void Update()
    {
        // 좌우 입력
        moveInput = Input.GetAxisRaw("Horizontal");

        // 이동 방향에 따라 캐릭터 방향 변경
        if (moveInput > 0)
        {
            facingDirection = 1;
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            facingDirection = -1;
            spriteRenderer.flipX = true;
        }

        // 이동 애니메이션
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        // Ground 체크
        isGrounded = Physics2D.OverlapCircle(
            (Vector2)transform.position + groundCheckOffset,
            groundCheckRadius,
            groundLayer
        );

        // Animator 전달
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // poopSound.PlayOneShot(poopSoundClips[1]); // 히히
        }

        // 대시
        if (Input.GetKeyDown(KeyCode.LeftShift) &&
            !isDashing &&
            canDash &&
            moveInput != 0)
        {
            StartCoroutine(Dash());
        }
    }


    private void FixedUpdate()
    {
        // 대시 중이 아닐 때 일반 이동
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(
                moveInput * moveSpeed,
                rb.linearVelocity.y
            );
        }
    }


    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        animator.SetTrigger("Dash");
        poopSound.PlayOneShot(poopSoundClips[1]); // 뿡

        // 바라보는 방향으로 대시
        rb.linearVelocity = new Vector2(
            dashSpeed * facingDirection,
            rb.linearVelocity.y
        );


        yield return new WaitForSeconds(dashDuration);


        isDashing = false;


        yield return new WaitForSeconds(dashCooldown);


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