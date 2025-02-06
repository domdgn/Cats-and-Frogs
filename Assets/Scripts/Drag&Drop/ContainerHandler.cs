using System.Collections.Generic;
using UnityEngine;

public class ContainerHandler : MonoBehaviour
{
    private static Dictionary<Vector2, GameObject> occupiedPositions = new Dictionary<Vector2, GameObject>();

    public static bool IsPositionOccupied(Vector2 position)
    {
        return occupiedPositions.ContainsKey(position);
    }

    public static void OccupyPosition(Vector2 position, GameObject obj)
    {
        occupiedPositions[position] = obj;
    }

    public static void ClearPosition(Vector2 position)
    {
        occupiedPositions.Remove(position);
    }
}