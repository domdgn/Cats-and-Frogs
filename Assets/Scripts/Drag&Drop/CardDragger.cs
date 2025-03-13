using UnityEngine;

public class CardDragger : MonoBehaviour
{
    [SerializeField] private CatSO catType;
    [SerializeField] private GameObject previewPrefab;

    private DragPreviewManager previewManager;
    private CatPlacementManager placementManager;

    void Start()
    {
        previewManager = FindObjectOfType<DragPreviewManager>();
        placementManager = FindObjectOfType<CatPlacementManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            if (touch.phase == TouchPhase.Began && DragManager.isDragAllowed)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    previewManager.StartDrag(previewPrefab, touchPosition, catType);
                }
            }

            if (touch.phase == TouchPhase.Ended && DragManager.isDragging)
            {
                previewManager.EndDrag(touchPosition, catType);
            }
        }
    }
}
