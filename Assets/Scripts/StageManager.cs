using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 꼭 필요함!

public class StageManager : MonoBehaviour
{
    // 버튼이 눌렸을 때 실행될 함수
    public void GoToStage1()
    {
        // "Stage1" 부분에 네가 실제 만든 스테이지 1 씬 이름을 정확히 적어줘
        SceneManager.LoadScene("Stage1"); 
    }
}