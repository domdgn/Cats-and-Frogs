using UnityEngine;

public static class DragManager
{
    public static bool isDragging = false;
    public static bool isDragAllowed = false;
    public static GameObject dragObject = null;

    public static void UpdateDragPermission(bool atShop)
    {
        isDragAllowed = atShop;
    }
}