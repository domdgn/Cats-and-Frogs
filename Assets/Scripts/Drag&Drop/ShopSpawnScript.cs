using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class ShopSpawnScript : MonoBehaviour
{
    [SerializeField] public CatSO catType;
    [SerializeField] public GameObject spawnPrefab;
    [SerializeField] GameObject previewPrefab;

    private static GameObject previewCat;
    private GameObject spawnedObject;
    private CameraController cameraController;
    private ProjectileFire gunScript;
    //private Melee script
    //private Coin collect script

    public Transform catParent;

    private static CatSO currentCatTypeForDeployment;

    private void Start()
    {
        catParent = GameObject.Find("Cats").transform;
        cameraController = FindObjectOfType<CameraController>();
    }
    public void InstantiatePrefab(Vector3 touchPosition)
    {
        spawnedObject = Instantiate(spawnPrefab, touchPosition, Quaternion.identity);
        if (spawnedObject != null)
        {
            Sprite sprite = catType.sprite;
            SpriteRenderer spriteRenderer = spawnedObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            DragManager.dragObject = spawnedObject;
            spawnedObject.layer = LayerMask.NameToLayer("UI");
            //Debug.Log($"Instantiated {spawnedObject.name} of type {catType.catName}");
        }
    }

    public void SpawnPreviewCat(Vector3 touchPosition)
    {
        currentCatTypeForDeployment = this.catType;

        previewCat = Instantiate(previewPrefab, touchPosition, Quaternion.identity);
        if (previewCat != null)
        {
            Sprite sprite = catType.sprite;
            SpriteRenderer spriteRenderer = previewCat.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            DragManager.dragObject = previewCat;
            previewCat.layer = LayerMask.NameToLayer("UI");
            //Debug.Log($"Preview {catType.catName}");

            if (CameraController.atShop)
            {
                //Debug.Log("Starting camera transition");
                cameraController.MoveCamera();
            }
        }
    }
    public void DeployCat(Vector2 gridPosition)
    {
        CatSO catToUse = currentCatTypeForDeployment;
        spawnedObject = Instantiate(spawnPrefab, gridPosition, Quaternion.identity, catParent);
        CatController controller = spawnedObject.GetComponent<CatController>();
        ProjectileFire gunScript = spawnedObject.GetComponent<ProjectileFire>();

        if (catToUse.catMode == CatType.Gun)
        {
            gunScript.enabled = true;
            //Debug.Log("GUN ENABLED!!!");
        }
        else
        {
            gunScript.enabled = false;
            //Debug.Log("GUN DISNABLED!!!");
        }

        if (controller != null)
        {
            controller.SetupCat(catToUse);
            //Debug.Log($"Deployed cat of type {catToUse.catName} at {gridPosition}");
        }
    }

    public void RemovePreviewCat()
    {
        if (previewCat != null)
        {
            Destroy(previewCat);
            previewCat = null;
        }
    }

    public GameObject GetSpawnedObject()
    {
        return previewCat;
    }
}