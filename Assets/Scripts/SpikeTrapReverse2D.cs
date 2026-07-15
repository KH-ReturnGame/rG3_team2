using UnityEngine;

public class SpikeTrapReverse2D : MonoBehaviour
{
    public enum MoveDirection { Vertical, Horizontal }

    [Header("함정 데미지 설정")]
    public int damageAmount = 1;

    [Header("스파이크 이동 설정 (역방향 출발)")]
    public bool isMoving = true;
    public MoveDirection direction = MoveDirection.Vertical;
    public float speed = 2f;
    public float moveDistance = 3f; // 이 거리만큼 아래 혹은 왼쪽으로 먼저 출발합니다!

    private Vector3 startLocalPosition;
    private bool movingForward = false; // 🌟 역방향(아래/왼쪽)으로 먼저 출발하도록 false 고정!

    void Start()
    {
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTrap();
        }
    }

    void MoveTrap()
    {
        if (direction == MoveDirection.Vertical)
        {
            // --- 세로(아래위) 로컬 왕복 ---
            float topLimit = startLocalPosition.y;
            float bottomLimit = startLocalPosition.y - moveDistance; // 시작점에서 아래로 내려감

            if (movingForward)
            {
                // 위로 올라가는 중
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + speed * Time.deltaTime, transform.localPosition.z);
                if (transform.localPosition.y >= topLimit)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, topLimit, transform.localPosition.z);
                    movingForward = false;
                }
            }
            else
            {
                // 아래로 내려가는 중 (시작 시 먼저 실행됨)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - speed * Time.deltaTime, transform.localPosition.z);
                if (transform.localPosition.y <= bottomLimit)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, bottomLimit, transform.localPosition.z);
                    movingForward = true;
                }
            }
        }
        else
        {
            // --- 가로(좌우) 로컬 왕복 ---
            float rightLimit = startLocalPosition.x;
            float leftLimit = startLocalPosition.x - moveDistance; // 시작점에서 왼쪽으로 먼저 감

            if (movingForward)
            {
                // 오른쪽으로 가는 중
                transform.localPosition = new Vector3(transform.localPosition.x + speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
                if (transform.localPosition.x >= rightLimit)
                {
                    transform.localPosition = new Vector3(rightLimit, transform.localPosition.y, transform.localPosition.z);
                    movingForward = false;
                }
            }
            else
            {
                // 왼쪽으로 가는 중 (시작 시 먼저 실행됨)
                transform.localPosition = new Vector3(transform.localPosition.x - speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
                if (transform.localPosition.x <= leftLimit)
                {
                    transform.localPosition = new Vector3(leftLimit, transform.localPosition.y, transform.localPosition.z);
                    movingForward = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHp = collision.GetComponent<PlayerHealth>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(damageAmount);
            }
        }
    }
}