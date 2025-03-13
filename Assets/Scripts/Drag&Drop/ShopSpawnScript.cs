using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawnScript : MonoBehaviour
{
    [SerializeField] public CatSO catType;
    [SerializeField] GameObject spawnPrefab;

    public bool hasCat;

    private static GameObject spawnedObject;
    private CameraController cameraController;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }
    public void InstantiatePrefab(Vector2 touchPosition)
    {
        spawnedObject = Instantiate(spawnPrefab, touchPosition, Quaternion.identity);
        if (spawnedObject != null)
        {
            hasCat = true;
            Sprite sprite = catType.sprite;
            SpriteRenderer spriteRenderer = spawnedObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            DragManager.dragObject = spawnedObject;
            spawnedObject.layer = LayerMask.NameToLayer("UI");
            Debug.Log($"Instantiated {spawnedObject.name} of type {catType.catName}");

            if (CameraController.atShop)
            {
                Debug.Log("Starting camera transition");
                cameraController.MoveCamera();
            }
        }
    }
    public void DeployCat(CatSO catType, Vector2 gridPosition)
    {
        spawnedObject = Instantiate(spawnPrefab, gridPosition, Quaternion.identity);
        CatController controller = spawnedObject.GetComponent<CatController>();
        //Debug.Log(catType.catName);
        if (controller != null)
        {
            controller.SetupCat(catType);
        }
    }

    public void DestroyCat()
    {
        hasCat = false;
        Destroy(spawnedObject);
    }
}
