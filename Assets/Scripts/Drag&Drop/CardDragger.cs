using UnityEngine;
public class CardDragger : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    private Vector3 touchPosition;
    private GameObject spawnedObject;
    private CameraController cameraController;
    private LayerMask gridLayer;

    [SerializeField] private CatSO catType;
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        gridLayer = LayerMask.GetMask("TileGrid");
    }

    void Update()
    {
        if (CameraController.atShop)
        {
            DragManager.isDragAllowed = true;
        }
        else
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
                    spawnedObject = Instantiate(spawnPrefab, touchPosition, Quaternion.identity);
                    if (spawnedObject != null)
                    {
                        Sprite sprite = catType.sprite;
                        SpriteRenderer spriteRenderer = spawnedObject.GetComponent<SpriteRenderer>();
                        spriteRenderer.sprite = sprite;

                        DragManager.dragObject = spawnedObject;
                        spawnedObject.layer = LayerMask.NameToLayer("UI");
                        Debug.Log($"Instantiated {spawnedObject.name}");

                        if (CameraController.atShop)
                        {
                            Debug.Log("Starting camera transition");
                            cameraController.MoveCamera();
                        }
                    }
                }
            }

            if (!DragManager.isDragAllowed)
            {
                Debug.LogWarning("Drag not permitted");
            }


            if (touch.phase == TouchPhase.Ended && DragManager.isDragging)
            {
                Debug.Log("Touch ended, attempting placement");
                DragManager.isDragging = false;
                if (spawnedObject != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, gridLayer);
                    if (hit.collider != null)
                    {
                        Vector2 gridPosition = hit.collider.transform.position;
                        if (!ContainerHandler.IsPositionOccupied(gridPosition))
                        {
                            Destroy(spawnedObject);
                            DeployCat(catType, gridPosition);

                            ContainerHandler.OccupyPosition(gridPosition, spawnedObject);
                            Debug.Log($"Placed {spawnedObject.name}");
                        }
                        else
                        {
                            Destroy(spawnedObject);
                            Debug.LogWarning("Position already occupied");
                        }
                    }
                    else
                    {
                        Destroy(spawnedObject);
                        Debug.LogWarning("Placement outside of grid not allowed");
                    }
                }
            }
        }
    }

    private void DeployCat(CatSO catType, Vector2 gridPosition)
    {
        spawnedObject = Instantiate(spawnPrefab, gridPosition, Quaternion.identity);
        CatController controller = spawnedObject.GetComponent<CatController>();
        //Debug.Log(catType.name);
        if (controller != null)
        {
            controller.SetupCat(catType);
        }
    }

    void LateUpdate()
    {
        if (DragManager.isDragging && spawnedObject != null && Input.touchCount > 0)
        {
            Vector3 TouchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            TouchPosition.z = 0;
            spawnedObject.transform.position = TouchPosition; 
        }
    }
}