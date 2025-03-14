using System.Collections.Generic;
using UnityEngine;

public class ContainerHandler : MonoBehaviour
{
    private static Dictionary<Vector2, GameObject> occupiedPositions = new Dictionary<Vector2, GameObject>();

    public static bool IsPositionOccupied(Vector2 position)
    {
        return occupiedPositions.ContainsKey(position);
    }

    public static GameObject WhoOccupies(Vector2 position)
    {
        if (occupiedPositions.ContainsKey(position))
        {
            return occupiedPositions[position];
        }
        return null;
    }

    public static void OccupyPosition(Vector2 position, GameObject obj)
    {
        occupiedPositions[position] = obj;
    }

    public static void ClearPosition(Vector2 position)
    {
        occupiedPositions.Remove(position);
    }

    public static void ClearAllPositions()
    {
        // Create a copy of the keys to iterate over
        List<Vector2> positionsToRemove = new List<Vector2>(occupiedPositions.Keys);

        foreach (Vector2 position in positionsToRemove)
        {
            ClearPosition(position);
            Destroy(occupiedPositions[position]);
            Debug.Log($"Position {position} cleared");
        }
    }
}