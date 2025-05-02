using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InputManager handles both touch and mouse input for cross-platform support.
/// Use this class for WebGL builds to support cursor interactions on desktop browsers.
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    // Events for different input phases
    public event Action<Vector3, InputData> OnInputBegan;
    public event Action<Vector3, InputData> OnInputMoved;
    public event Action<Vector3, InputData> OnInputEnded;

    private GameObject currentTouchedObject;
    private RaycastHit2D[] hitResults = new RaycastHit2D[10];
    private bool isMouseDown = false;
    private bool wasMouseMoving = false;
    private Vector3 lastMousePosition = Vector3.zero;

    // Structure to normalize touch/mouse data
    public struct InputData
    {
        public Vector2 position;
        public Vector2 deltaPosition;
        public float deltaTime;
        public bool isMouse;

        public static InputData FromTouch(Touch touch)
        {
            return new InputData
            {
                position = touch.position,
                deltaPosition = touch.deltaPosition,
                deltaTime = touch.deltaTime,
                isMouse = false
            };
        }

        public static InputData FromMouse(Vector2 position, Vector2 deltaPosition, float deltaTime)
        {
            return new InputData
            {
                position = position,
                deltaPosition = deltaPosition,
                deltaTime = deltaTime,
                isMouse = true
            };
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // First check for touch input (prioritize over mouse)
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        // If no touch input, check for mouse input
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        touchPosition.z = 0;

        InputData inputData = InputData.FromTouch(touch);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                currentTouchedObject = GetTouchedObject(touchPosition);
                OnInputBegan?.Invoke(touchPosition, inputData);
                break;

            case TouchPhase.Moved:
                OnInputMoved?.Invoke(touchPosition, inputData);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                OnInputEnded?.Invoke(touchPosition, inputData);
                currentTouchedObject = null;
                break;
        }
    }

    private void HandleMouseInput()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        Vector2 mouseDelta = mouseScreenPosition - lastMousePosition;
        bool isMoving = mouseDelta.magnitude > 0.01f;

        InputData inputData = InputData.FromMouse(
            mouseScreenPosition,
            mouseDelta,
            Time.deltaTime
        );

        // Mouse button pressed this frame
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            currentTouchedObject = GetTouchedObject(mouseWorldPosition);
            OnInputBegan?.Invoke(mouseWorldPosition, inputData);
        }
        // Mouse is being held down and moving
        else if (isMouseDown && isMoving)
        {
            wasMouseMoving = true;
            OnInputMoved?.Invoke(mouseWorldPosition, inputData);
        }
        // Mouse button released this frame
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            OnInputEnded?.Invoke(mouseWorldPosition, inputData);
            currentTouchedObject = null;
            wasMouseMoving = false;
        }

        lastMousePosition = mouseScreenPosition;
    }

    public GameObject GetTouchedObject(Vector3 position)
    {
        int hitCount = Physics2D.RaycastNonAlloc(position, Vector2.zero, hitResults);
        if (hitCount <= 0) return null;
        return hitResults[0].collider?.gameObject;
    }

    public bool RaycastOnLayer(Vector3 position, LayerMask layerMask, out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, layerMask);
        return hit.collider != null;
    }

    public bool IsObjectTouched(GameObject obj, Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == obj;
    }

    // Get the current input position in world coordinates
    public Vector3 GetCurrentInputPosition()
    {
        if (Input.touchCount > 0)
        {
            return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}