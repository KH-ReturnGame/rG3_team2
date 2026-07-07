using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Rigidbody2D를 저장할 변수
    // 플레이어의 물리 움직임을 담당함
    Rigidbody2D rb;

    // 현재 이동 방향 저장 변수
    // -1 = 왼쪽
    //  0 = 정지
    //  1 = 오른쪽
    float moveInput;

    // 플레이어 이동 속도
    // public이라 Unity Inspector에서 수정 가능
    public float moveSpeed = 5f;

    // 점프 힘
    public float jumpForce = 20f;

    // 현재 바닥 위에 있는지 확인
    bool isGround;
    // 현재 벽에 닿아있는지 확인
    bool isWall;

    // 현재 벽 슬라이드 중인지 확인
    bool isWallSliding;

    // 벽에서 떨어지는 속도 제한
    public float wallSlideSpeed = 2f;

    void Start()
    {
        // Player 오브젝트에 붙어있는 Rigidbody2D 가져오기
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        // 매 프레임마다 입력 초기화
        // 방향키 안 누르면 자동으로 0(정지)
        moveInput = 0;

        // 왼쪽 방향키를 누르면
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // 왼쪽 방향 입력
            moveInput = -1;
        }

        // 오른쪽 방향키를 누르면
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // 오른쪽 방향 입력
            moveInput = 1;
        }

        // 점프 입력

        // Space를 눌렀고
        // 현재 바닥 위에 있다면 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            // 현재 x속도는 유지하고
            // y속도만 jumpForce로 변경
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // 점프했으므로 공중 상태로 변경
            isGround = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding)
        {
            // 오른쪽 벽에 붙은 상태에서
            // 왼쪽 방향키 + 점프
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.linearVelocity = new Vector2(-20f, 20f);
            }

            // 왼쪽 벽에 붙은 상태에서
            // 오른쪽 방향키 + 점프
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.linearVelocity = new Vector2(20f, 20f);
            }

            // 벽 상태 잠깐 해제
            isWall = false;
            isWallSliding = false;
        }

        // ------------------------
        // 벽 슬라이드 조건 확인
        // ------------------------

        // 조건:
        // 1. 벽에 닿아있고
        // 2. 바닥에 없고
        // 3. 아래로 떨어지는 중이고
        // 4. 방향키를 누르고 있다면

        if (isWall &&
            !isGround)
        {
            // 벽 슬라이드 상태 시작
            isWallSliding = true;
        }
        else
        {
            // 조건 아니면 종료
            isWallSliding = false;
        }


    }

    void FixedUpdate()
    {
        // 벽 슬라이드 중이라면
        if (isWallSliding)
        {
            // x축 이동 멈춤
            // y축만 천천히 떨어짐
            rb.linearVelocity =
                new Vector2(0, -wallSlideSpeed);
        }
        else
        {
            // 일반 이동
            rb.linearVelocity =
                new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트의 Tag가 Ground라면
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 바닥 상태 true
            isGround = true;
        }
    }

    // 벽 충돌 시작
    // ------------------------

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌 중인 오브젝트가 Wall 태그라면
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 벽 상태 true
            isWall = true;
        }
    }

    // 충돌 종료
    // ------------------------

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Wall 태그에서 떨어졌다면
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 벽 상태 false
            isWall = false;

            // 벽 슬라이드 상태도 종료
            isWallSliding = false;
        }
    }

}