using UnityEngine;

public class LogoPulse : MonoBehaviour
{
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public float pulseSpeed = 1.0f;
    public float easeAmount = 0.5f;

    public float wiggleAmount = 5.0f; // Degrees of wiggle
    public float wiggleSpeed = 2.0f;

    private Vector3 initialScale;
    private Quaternion initialRotation;

    void Start()
    {
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // --- Scaling ---
        float pingPongValue = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
        float easedValue = Mathf.SmoothStep(0.0f, 1.0f, pingPongValue);
        float scaleFactor = Mathf.Lerp(minScale, maxScale, easedValue);
        transform.localScale = initialScale * scaleFactor;

        // --- Rotation Wiggle ---
        float wiggleOffset = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        transform.rotation = initialRotation * Quaternion.Euler(0, 0, wiggleOffset);
    }
}