using System.Collections;
using UnityEngine;

public class CardDragger : MonoBehaviour
{
    private CameraController cameraController;
    private LayerMask gridLayer;
    private ShopSpawnScript shopSpawnScript;
    [SerializeField] private int cost;
    private SpriteRenderer backgroundSprite;
    private bool canAfford;

    // Add a unique ID to each card dragger
    private static int nextInstanceId = 0;
    private int instanceId;

    void Awake()
    {
        instanceId = nextInstanceId++;

        CurrencyManager currencyMgr = FindObjectOfType<CurrencyManager>();
        if (currencyMgr != null)
        {
            currencyMgr.OnBalanceUpdated += UpdateCardInteractability;
        }

        backgroundSprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(WaitForTouchManager());
    }

    IEnumerator WaitForTouchManager()
    {
        while (TouchManager.Instance == null)
        {
            Debug.LogWarning("Waiting for TouchManager...");
            yield return null;
        }

        Debug.Log("TouchManager found, subscribing to events.");
        TouchManager.Instance.OnTouchBegan += HandleTouchBegan;
        TouchManager.Instance.OnTouchEnded += HandleTouchEnded;
    }

    void OnDisable()
    {
        // Unsubscribe from touch events
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.OnTouchBegan -= HandleTouchBegan;
            TouchManager.Instance.OnTouchEnded -= HandleTouchEnded;
        }
    }

    void UpdateCardInteractability(int balance)
    {
        if (balance < cost)
        {
            canAfford = false;
            backgroundSprite.color = Color.gray;
        }
        else
        {
            canAfford = true;
            backgroundSprite.color = Color.yellow;
        }
    }

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        shopSpawnScript = GetComponent<ShopSpawnScript>();
        gridLayer = LayerMask.GetMask("TileGrid");
    }

    private void HandleTouchBegan(Vector3 touchPosition, Touch touch)
    {
        //Debug.Log("Card Dragger touch begin handler");
        if (!CameraController.atShop || !DragManager.isDragAllowed)
            return;

        if (TouchManager.Instance.IsObjectTouched(gameObject, touchPosition) && canAfford)
        {
            // Remember which card started the drag
            DragManager.isDragging = true;
            DragManager.currentDraggingId = instanceId;
            shopSpawnScript.SpawnPreviewCat(touchPosition);
        }
    }

    private void HandleTouchEnded(Vector3 touchPosition, Touch touch)
    {
        //Debug.Log("Card Dragger touch end handler");
        if (!DragManager.isDragging || DragManager.currentDraggingId != instanceId)
            return;

        RaycastHit2D hit;
        if (TouchManager.Instance.RaycastOnLayer(touchPosition, gridLayer, out hit))
        {
            Vector2 gridPosition = hit.collider.transform.position;
            if (!ContainerHandler.IsPositionOccupied(gridPosition))
            {
                DragManager.isDragging = false;
                shopSpawnScript.RemovePreviewCat();
                shopSpawnScript.DeployCat(gridPosition);
                CurrencyManager.Instance.SpendMoney(cost);
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

    void Update()
    {
        if (!CameraController.atShop)
        {
            DragManager.isDragAllowed = false;
        }
    }

    void LateUpdate()
    {
        // Only move the preview if this is the card that started the drag
        if (DragManager.isDragging && DragManager.currentDraggingId == instanceId && Input.touchCount > 0)
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