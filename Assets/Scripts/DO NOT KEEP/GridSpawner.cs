using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, -Camera.main.transform.position.z));
            worldPosition.z = 0;

            LayerMask gridLayerMask = LayerMask.GetMask("TileGrid");
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, gridLayerMask);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("GridSquare"))
            {
                Transform gridSquare = hit.collider.transform;

                if (gridSquare.childCount == 1 || gridSquare.childCount == 0)
                {
                    GameObject spawned = Instantiate(prefabToSpawn, gridSquare.position, Quaternion.identity);
                    spawned.transform.parent = gridSquare;
                    Debug.Log($"Tile placed at time {Time.time}");
                }
                else
                {
                    foreach (Transform child in gridSquare)
                    {
                        if (child.CompareTag("Cat"))
                        {
                            Debug.Log($"Tile destroyed at time {Time.time}");
                            Destroy(child.gameObject);
                        }
                    }
                }
            }
        }
    }
}