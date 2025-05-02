using System.Collections;
using UnityEngine;

public class CardDragger : MonoBehaviour
{
    private CameraController cameraController;
    private InputManager inputManager;
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
        backgroundSprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(WaitForDependencies());
    }

    IEnumerator WaitForDependencies()
    {
        // Wait for InputManager to initialize
        while (InputManager.Instance == null)
        {
            Debug.LogWarning("Waiting for InputManager...");
            yield return null;
        }

        // Cache reference to InputManager
        inputManager = InputManager.Instance;

        // Wait for CurrencyManager to initialize
        while (CurrencyManager.Instance == null)
        {
            Debug.LogWarning("Waiting for Currency Manager...");
            yield return null;
        }

        //Debug.Log("InputManager found, subscribing to events.");
        inputManager.OnInputBegan += HandleInputBegan;
        inputManager.OnInputEnded += HandleInputEnded;

        CurrencyManager.Instance.OnBalanceUpdated += UpdateCardInteractability;
        yield return null;
        CurrencyManager.Instance.UpdateCurrency();
    }

    void OnDisable()
    {
        // Unsubscribe from events
        if (inputManager != null)
        {
            inputManager.OnInputBegan -= HandleInputBegan;
            inputManager.OnInputEnded -= HandleInputEnded;
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnBalanceUpdated -= UpdateCardInteractability;
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

    private void HandleInputBegan(Vector3 inputPosition, InputManager.InputData inputData)
    {
        //Debug.Log("Card Dragger input begin handler");
        if (!CameraController.atShop || !DragManager.isDragAllowed)
            return;

        if (inputManager.IsObjectTouched(gameObject, inputPosition) && canAfford)
        {
            // Remember which card started the drag
            DragManager.isDragging = true;
            DragManager.currentDraggingId = instanceId;
            shopSpawnScript.SpawnPreviewCat(inputPosition);
        }
    }

    private void HandleInputEnded(Vector3 inputPosition, InputManager.InputData inputData)
    {
        //Debug.Log("Card Dragger input end handler");
        if (!DragManager.isDragging || DragManager.currentDraggingId != instanceId)
            return;

        RaycastHit2D hit;
        if (inputManager.RaycastOnLayer(inputPosition, gridLayer, out hit))
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
        if (DragManager.isDragging && DragManager.currentDraggingId == instanceId)
        {
            // Get current input position regardless of input type (touch or mouse)
            Vector3 inputPosition = inputManager.GetCurrentInputPosition();
            inputPosition.z = 0;

            GameObject spawnedObject = shopSpawnScript.GetSpawnedObject();
            if (spawnedObject != null)
            {
                spawnedObject.transform.position = inputPosition;
            }
        }
    }
}