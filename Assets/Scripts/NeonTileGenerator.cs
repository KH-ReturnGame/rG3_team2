using UnityEngine;
using System.IO;

public class NeonTileGenerator : MonoBehaviour
{
    [ContextMenu("Generate Solid Light Red Tile")]
    void GenerateSolidLightRedTile()
    {
        int size = 16;
        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point; // 픽셀 뭉개짐 방지

        // 내부와 테두리 구분 없이 전체를 채울 연빨강색 (RGB: 255, 140, 140 느낌)
        Color solidColor = new Color(1f, 0.55f, 0.55f, 1f);   

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // 구분 없이 모든 픽셀을 연빨강색으로 채움
                texture.SetPixel(x, y, solidColor);
            }
        }
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Tile_SolidLightRed_16x16.png", bytes);
        
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // 유니티 에디터 새로고침
#endif
        
        Debug.Log("전체 연빨강 타일 생성 완료: Tile_SolidLightRed_16x16.png");
    }
}