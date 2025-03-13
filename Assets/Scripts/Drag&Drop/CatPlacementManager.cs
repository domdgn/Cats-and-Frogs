using UnityEngine;

public class CatPlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject catPrefab;
    private LayerMask gridLayer;

    void Start()
    {
        gridLayer = LayerMask.GetMask("TileGrid");
    }

    public void AttemptPlacement(Vector3 position, CatSO catType, GameObject previewObject)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, gridLayer);

        if (hit.collider != null)
        {
            Vector2 gridPosition = hit.collider.transform.position;

            if (!ContainerHandler.IsPositionOccupied(gridPosition))
            {
                Destroy(previewObject);
                GameObject placedCat = DeployCat(catType, gridPosition);
                ContainerHandler.OccupyPosition(gridPosition, placedCat);
                Debug.Log($"Placed {placedCat.name}");
            }
            else
            {
                Destroy(previewObject);
                Debug.LogWarning("Position already occupied");
            }
        }
        else
        {
            Destroy(previewObject);
            Debug.LogWarning("Placement outside of grid not allowed");
        }
    }

    private GameObject DeployCat(CatSO catType, Vector2 gridPosition)
    {
        GameObject deployedCat = Instantiate(catPrefab, gridPosition, Quaternion.identity);
        CatController controller = deployedCat.GetComponent<CatController>();

        if (controller != null)
        {
            controller.SetupCat(catType);
        }

        return deployedCat;
    }
}