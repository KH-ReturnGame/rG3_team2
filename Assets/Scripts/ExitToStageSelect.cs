using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필수!

public class ExitToStageSelect : MonoBehaviour
{
    // 이동할 스테이지 선택창 씬의 이름을 유니티 인스펙터에서 적어줄 변수
    public string stageSelectSceneName = "StageSelect"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 밟은 오브젝트가 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 진입! 스테이지 선택창으로 돌아갑니다.");
            
            // 지정한 이름의 씬으로 이동
            SceneManager.LoadScene(stageSelectSceneName);
        }
    }
}