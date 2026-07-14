using UnityEngine;
using System.IO;

public class NeonTileGenerator : MonoBehaviour
{
    [ContextMenu("Generate Solid Dark Red Tile")]
    void GenerateSolidDarkRedTile()
    {
        int size = 16;
        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point; // 픽셀 뭉개짐 방지

        // 전체를 채울 어두운 빨강색 (RGB: 약 130, 0, 0)
        Color solidColor = new Color(0.5f, 0f, 0f, 1f);   

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // 모든 픽셀을 어두운 빨강색으로 채움
                texture.SetPixel(x, y, solidColor);
            }
        }
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Tile_SolidDarkRed_16x16.png", bytes);
        
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // 유니티 에디터 새로고침
#endif
        
        Debug.Log("전체 어두운 빨강 타일 생성 완료: Tile_SolidDarkRed_16x16.png");
    }
}