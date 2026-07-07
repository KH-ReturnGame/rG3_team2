using UnityEngine;
using System.IO;

public class NeonTileGenerator : MonoBehaviour
{
    [ContextMenu("Generate Green Neon Tile")]
    void GenerateGreenTile()
    {
        int size = 16;
        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point; // 픽셀 뭉개짐 방지

        // 초기 버전과 동일하게 단색으로 지정
        Color fillColor = new Color(0.4f, 0.4f, 0.4f, 1f); // 내부 꽉 찬 회색 박스
        Color borderColor = new Color(0f, 1f, 0.3f, 1f);   // 외곽선 초록색 네온 컬러

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // 맨 가장자리 1픽셀씩만 초록색 테두리 칠하기
                if (x == 0 || x == size - 1 || y == 0 || y == size - 1)
                {
                    texture.SetPixel(x, y, borderColor);
                }
                else
                {
                    texture.SetPixel(x, y, fillColor);
                }
            }
        }
        texture.Apply();

        // 파일명을 Green으로 완전히 분리하여 저장
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/NeonTile_Green_16x16.png", bytes);
        
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // 유니티 에디터 새로고침
#endif
        
        Debug.Log("새로운 파일로 생성 완료: NeonTile_Green_16x16.png");
    }
}