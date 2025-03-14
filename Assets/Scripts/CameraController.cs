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
        DragManager.isDragAllowed = false;
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

        StartCoroutine(SmoothCameraMovement(distanceToMove));
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

        isMoving = false;
        DragManager.isDragAllowed = true;
    }
    public float GetMoveDuration()
    {
        return moveDuration;
    }

    public bool isAtShop()
    {
        return atShop;
    }
}