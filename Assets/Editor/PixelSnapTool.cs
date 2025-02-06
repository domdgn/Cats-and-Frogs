using UnityEngine;
using UnityEditor;

public class PixelSnapTool : EditorWindow
{
    const float PPU = 32f;

    [MenuItem("Tools/Snap Selected to Pixel Grid")]
    static void SnapToGrid()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Vector3 pos = obj.transform.position;
            obj.transform.position = new Vector3(
                Mathf.Round(pos.x * PPU) / PPU,
                Mathf.Round(pos.y * PPU) / PPU,
                pos.z
            );
        }
    }
}