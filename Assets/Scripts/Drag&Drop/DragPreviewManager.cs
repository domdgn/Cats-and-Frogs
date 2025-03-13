using UnityEngine;

public class DragPreviewManager : MonoBehaviour
{
    private GameObject dragPreview;
    private CameraController cameraController;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    public void StartDrag(GameObject previewPrefab, Vector3 position, CatSO catType)
    {
        DragManager.isDragging = true;
        dragPreview = Instantiate(previewPrefab, position, Quaternion.identity);

        if (dragPreview != null)
        {
            // Set up the preview sprite
            SpriteRenderer spriteRenderer = dragPreview.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = catType.sprite;

            // Set as the current drag object
            DragManager.dragObject = dragPreview;
            dragPreview.layer = LayerMask.NameToLayer("UI");
            Debug.Log($"Instantiated preview: {dragPreview.name}");

            // Handle camera transition
            if (CameraController.atShop)
            {
                Debug.Log("Starting camera transition");
                cameraController.MoveCamera();
            }
        }
    }

    public void EndDrag(Vector3 position, CatSO catType)
    {
        Debug.Log("Touch ended, attempting placement");
        DragManager.isDragging = false;

        if (dragPreview != null)
        {
            FindObjectOfType<CatPlacementManager>().AttemptPlacement(position, catType, dragPreview);
        }
    }

    void LateUpdate()
    {
        if (DragManager.isDragging && dragPreview != null && Input.touchCount > 0)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            touchPosition.z = 0;
            dragPreview.transform.position = touchPosition;
        }
    }
}