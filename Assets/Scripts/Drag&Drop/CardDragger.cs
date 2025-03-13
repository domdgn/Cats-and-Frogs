using UnityEngine;
public class CardDragger : MonoBehaviour
{
    private Vector3 touchPosition;
    private CameraController cameraController;
    private LayerMask gridLayer;
    private ShopSpawnScript shopSpawnScript;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        shopSpawnScript = GetComponent<ShopSpawnScript>();
        gridLayer = LayerMask.GetMask("TileGrid");
    }

    void Update()
    {
        if (!CameraController.atShop)
        {
            DragManager.isDragAllowed = false;
        }

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
                    shopSpawnScript.SpawnPreviewCat(touchPosition);
                }
            }

            if (!DragManager.isDragAllowed)
            {
                Debug.LogWarning("Drag not permitted");
            }


            if (touch.phase == TouchPhase.Ended && DragManager.isDragging)
            {
                if (DragManager.isDragging)
                {
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, gridLayer);
                    if (hit.collider != null)
                    {
                        Vector2 gridPosition = hit.collider.transform.position;
                        if (!ContainerHandler.IsPositionOccupied(gridPosition))
                        {
                            DragManager.isDragging = false;
                            shopSpawnScript.RemovePreviewCat();
                            shopSpawnScript.DeployCat(gridPosition);

                            ContainerHandler.OccupyPosition(gridPosition, shopSpawnScript.GetSpawnedObject());
                        }
                        else
                        {
                            DragManager.isDragging = false;
                            shopSpawnScript.RemovePreviewCat();
                            Debug.LogWarning("Position already occupied");
                        }
                    }
                    else
                    {
                        DragManager.isDragging = false;
                        shopSpawnScript.RemovePreviewCat();
                        Debug.LogWarning("Placement outside of grid not allowed");
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        if (DragManager.isDragging && Input.touchCount > 0)
        {
            Vector3 TouchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            TouchPosition.z = 0;
            GameObject spawnedObject = shopSpawnScript.GetSpawnedObject();
            if (spawnedObject != null)
            {
                spawnedObject.transform.position = TouchPosition;
            }
        }
    }
}