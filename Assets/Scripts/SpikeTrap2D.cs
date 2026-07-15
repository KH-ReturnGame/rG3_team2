using UnityEngine;

public class SpikeTrap2D : MonoBehaviour
{
    public enum MoveDirection { Vertical, Horizontal }

    [Header("함정 데미지 설정")]
    public int damageAmount = 1;

    [Header("스파이크 이동 설정")]
    public bool isMoving = true;
    public MoveDirection direction = MoveDirection.Vertical;
    public float speed = 2f;
    public float moveDistance = 3f; // 이제 이 값이 정직하게 미터(Unit) 단위로 작동합니다!

    private Vector3 startLocalPosition; // 로컬 시작 위치 저장
    private bool movingForward = true;

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
            // --- 세로(위아래) 로컬 왕복 ---
            float topLimit = startLocalPosition.y + moveDistance;
            float bottomLimit = startLocalPosition.y;

            if (movingForward)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + speed * Time.deltaTime, transform.localPosition.z);
                if (transform.localPosition.y >= topLimit)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, topLimit, transform.localPosition.z);
                    movingForward = false;
                }
            }
            else
            {
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
            float rightLimit = startLocalPosition.x + moveDistance;
            float leftLimit = startLocalPosition.x;

            if (movingForward)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
                if (transform.localPosition.x >= rightLimit)
                {
                    transform.localPosition = new Vector3(rightLimit, transform.localPosition.y, transform.localPosition.z);
                    movingForward = false;
                }
            }
            else
            {
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