using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 8f;
    public bool isSpeedBoosted = false;

    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public LayerMask groundLayer;       // 여전히 타일맵 레이어는 Ground로 맞춰야 해!
    private bool isGrounded;

    [Header("Dash Settings")]
    public float dashDistanceInTiles = 6f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider; // 추가됨
    private float horizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>(); // 플레이어의 박스 콜라이더 가져오기
    }

    void Update()
    {
        if (isDashing) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        // [변경됨] 플레이어 콜라이더 발밑으로 레이저를 살짝 쏴서 바닥 체크 (GroundCheck 오브젝트 필요 없음!)
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);
        isGrounded = raycastHit.collider != null;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DashRoutine());
        }

        FlipSprite();
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = false;

        float dashDirection = horizontal != 0 ? Mathf.Sign(horizontal) : (transform.localScale.x > 0 ? 1f : -1f);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float targetDashSpeed = dashDistanceInTiles / dashDuration;
        rb.linearVelocity = new Vector2(dashDirection * targetDashSpeed, 0f);

        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        rb.gravityScale = originalGravity;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void FlipSprite()
    {
        if (horizontal > 0f) transform.localScale = new Vector3(1f, 1f, 1f);
        else if (horizontal < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}