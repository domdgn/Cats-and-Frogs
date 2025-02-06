using UnityEngine;

public class CardDragger : MonoBehaviour
{
    public GameObject spawnPrefab;
    private Vector3 touchPosition;
    private GameObject spawnedObject;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            if (touch.phase == TouchPhase.Began && DragManager.isDragAllowed)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    DragManager.isDragging = true;
                    spawnedObject = Instantiate(spawnPrefab, touchPosition, Quaternion.identity);
                    DragManager.dragObject = spawnedObject;
                    Debug.Log($"Instantiated {spawnedObject.name}");
                }
            }

            if (!DragManager.isDragAllowed)
            {
                Debug.Log("Drag not permitted");
            }

            if (DragManager.isDragging && DragManager.isDragAllowed && touch.phase == TouchPhase.Moved)
            {
                spawnedObject.layer = LayerMask.NameToLayer("UI");
                spawnedObject.transform.position = touchPosition;
            }

            if (DragManager.isDragging && touch.phase == TouchPhase.Ended)
            {
                DragManager.isDragging = false;
                LayerMask gridLayer = LayerMask.GetMask("TileGrid");
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, gridLayer);
                if (hit.collider != null)
                {
                    Vector2 gridPosition = hit.collider.transform.position;
                    if (!ContainerHandler.IsPositionOccupied(gridPosition))
                    {
                        Destroy(spawnedObject);
                        spawnedObject = Instantiate(spawnPrefab, gridPosition, Quaternion.identity);
                        ContainerHandler.OccupyPosition(gridPosition, spawnedObject);
                        Debug.Log($"Placed {spawnedObject.name}");
                    }
                    else
                    {
                        Destroy(spawnedObject);
                        Debug.Log("Position already occupied");
                    }
                }
            }
        }
    }
}