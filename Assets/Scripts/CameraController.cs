using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveDistance = 6.75f;
    [SerializeField] private float moveDuration = 0.2f; // Duration of movement (sec)
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Camera mainCamera;
    public static bool atShop;
    private bool isMoving = false;

    private void Start()
    {
        atShop = false;
        mainCamera = Camera.main;
    }

    public void MoveCamera()
    {
        // Don't start a new movement if already moving
        if (isMoving)
            return;

        float distanceToMove;
        if (atShop)
        {
            distanceToMove = (-1) * moveDistance;
        }
        else
        {
            distanceToMove = moveDistance;
        }

        // Start the smooth movement coroutine
        StartCoroutine(SmoothCameraMovement(distanceToMove));

        // Toggle the shop state
        atShop = !atShop;
    }

    private IEnumerator SmoothCameraMovement(float distanceToMove)
    {
        isMoving = true;

        Vector3 startPosition = mainCamera.transform.position;
        Vector3 targetPosition = new Vector3(
            startPosition.x + distanceToMove,
            startPosition.y,
            startPosition.z
        );

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            float easedT = movementCurve.Evaluate(t);
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, easedT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;

        Debug.Log($"Camera moved {distanceToMove} units in X direction");
        isMoving = false;
    }
    public float GetMoveDuration()
    {
        return moveDuration;
    }
}