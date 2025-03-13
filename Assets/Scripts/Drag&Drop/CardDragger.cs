using UnityEngine;
public class CardDragger : MonoBehaviour
{
    //[SerializeField] private GameObject spawnPrefab;
    private Vector3 touchPosition;
    private static GameObject spawnedObject;
    private CameraController cameraController;
    private LayerMask gridLayer;
    private ShopSpawnScript shopSpawnScript;

    //[SerializeField] private CatSO catType;
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
                    shopSpawnScript.InstantiatePrefab(touchPosition);
                }
            }

            if (!DragManager.isDragAllowed)
            {
                Debug.LogWarning("Drag not permitted");
            }


            if (touch.phase == TouchPhase.Ended && DragManager.isDragging)
            {
                DragManager.isDragging = false;
                if (shopSpawnScript.hasCat)
                {
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, gridLayer);
                    if (hit.collider != null)
                    {
                        Vector2 gridPosition = hit.collider.transform.position;
                        if (!ContainerHandler.IsPositionOccupied(gridPosition))
                        {
                            shopSpawnScript.DestroyCat();
                            shopSpawnScript.DeployCat(shopSpawnScript.catType, gridPosition);

                            ContainerHandler.OccupyPosition(gridPosition, spawnedObject);
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

    //private void DeployCat(CatSO catType, Vector2 gridPosition)
    //{
    //    spawnedObject = Instantiate(spawnPrefab, gridPosition, Quaternion.identity);
    //    CatController controller = spawnedObject.GetComponent<CatController>();
    //    //Debug.Log(catType.catName);
    //    if (controller != null)
    //    {
    //        controller.SetupCat(catType);
    //    }
    //}

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