using UnityEngine;
using System.Collections;

public class SpeedPad : MonoBehaviour
{
    public float speedMultiplier = 3.0f; // 체감이 확 되도록 기본 배율을 3~4배로 올려봐!
    public float maintainTime = 1.25f;   // 발판을 벗어나도 1.25초 동안 유지

    private Coroutine resetCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            
            if (playerMove != null)
            {
                // 타일 사이 경계선 버그 때문에 도중에 켜진 타이머가 있다면 즉시 취소
                if (resetCoroutine != null)
                {
                    StopCoroutine(resetCoroutine);
                    resetCoroutine = null;
                }

                // 아직 가속 상태가 아닐 때만 속도를 곱해줌
                if (!playerMove.isSpeedBoosted) 
                {
                    playerMove.moveSpeed *= speedMultiplier;
                    playerMove.isSpeedBoosted = true;
                    Debug.Log($"가속 발판 작동! 현재 속도: {playerMove.moveSpeed}");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            
            if (playerMove != null)
            {
                // 기존에 돌던 타이머가 있다면 중복 방지를 위해 끄고 새로 시작
                if (resetCoroutine != null)
                {
                    StopCoroutine(resetCoroutine);
                }
                
                // 1.25초 뒤에 속도를 원래대로 돌리는 코루틴 실행
                resetCoroutine = StartCoroutine(ResetSpeedAfterDelay(playerMove));
            }
        }
    }

    private IEnumerator ResetSpeedAfterDelay(PlayerMove playerMove)
    {
        yield return new WaitForSeconds(maintainTime);

        if (playerMove != null && playerMove.isSpeedBoosted)
        {
            // 가속 해제 시 원래 속도로 안전하게 복구
            playerMove.moveSpeed /= speedMultiplier;
            playerMove.isSpeedBoosted = false;
            Debug.Log("가속 유지 시간 종료! 원래 속도로 복귀.");
        }

        resetCoroutine = null;
    }
}