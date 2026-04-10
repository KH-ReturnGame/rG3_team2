using UnityEngine;

// 파일 이름도 반드시 PlayerMove.cs
public class PlayerMove : MonoBehaviour
{
    // 이동 속도
    public float moveSpeed = 5f;

    // Rigidbody2D 저장
    private Rigidbody2D rb;

    // Animator (애니메이션용)
    private Animator anim;

    void Start()
    {
        // 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move(); // 이동 함수 호출 (이거 없으면 안 움직임)
    }

    void Move()
    {
        // 좌우 입력 받기 (A/D, ←/→)
        float moveInput = Input.GetAxisRaw("Horizontal");
        Debug.Log(moveInput);
        // 이동 처리 (Unity 6 기준 → linearVelocity 사용)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 애니메이션 (움직이는지 여부 전달)
        if (anim != null)
        {
            anim.SetFloat("run", Mathf.Abs(moveInput));
        }
    }
}

