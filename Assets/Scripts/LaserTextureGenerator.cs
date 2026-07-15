using UnityEngine;
using System.IO;

public class LaserTextureGenerator : MonoBehaviour
{
    // 컴포넌트 우측 점 3개(⋮) 버튼을 누르면 이 메뉴가 나타납니다!
    [ContextMenu("Generate Pure Laser Sprite")]
    void GenerateLaser()
    {
        int width = 16;
        int height = 8;
        
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point; // 픽셀아트용 세팅

        // 1. 전체를 완전 투명(빈 공간)으로 채우기
        Color transparent = new Color(0f, 0f, 0f, 0f);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, y, transparent);
            }
        }

        // 2. 장치 없이, 처음부터 끝까지(x: 0 ~ 15) 레이저 줄기로만 꽉 채우기
        Color laserCenter = new Color(1f, 0.8f, 0.8f, 1f);   // 중심부 (가장 밝은 네온)
        Color laserBody = new Color(1f, 0.0f, 0.0f, 1f);     // 중간부 (선명한 빨강)
        Color laserEdge = new Color(0.5f, 0.0f, 0.0f, 0.8f); // 외곽선 (진한 테두리)

        for (int x = 0; x < width; x++)
        {
            texture.SetPixel(x, 1, laserEdge); // 위쪽 끝 테두리
            texture.SetPixel(x, 2, laserBody);
            
            texture.SetPixel(x, 3, laserCenter); // 밝은 중심부
            texture.SetPixel(x, 4, laserCenter);
            
            texture.SetPixel(x, 5, laserBody);
            texture.SetPixel(x, 6, laserEdge); // 아래쪽 끝 테두리
        }

        texture.Apply();

        // Assets 폴더 바로 아래에 'Laser_Body_16x8.png' 이름으로 저장
        byte[] bytes = texture.EncodeToPNG();
        string filePath = Path.Combine(Application.dataPath, "Laser_Body_16x8.png");
        File.WriteAllBytes(filePath, bytes);
        
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // 유니티 프로젝트 창 자동 새로고침
#endif
        
        Debug.Log($"★ 순수 레이저 줄기 생성 완료! 위치: Assets/Laser_Body_16x8.png");
    }
}