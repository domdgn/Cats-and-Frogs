using UnityEngine;

public class raySpin : MonoBehaviour
{
    public float rotationSpeed = 30.0f; // Degrees per second

    void Update()
    {
        // Rotate the image around its local Z-axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
