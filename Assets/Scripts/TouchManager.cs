using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance { get; private set; }

    // Events for different touch phases
    public event Action<Vector3, Touch> OnTouchBegan;
    public event Action<Vector3, Touch> OnTouchMoved;
    public event Action<Vector3, Touch> OnTouchEnded;

    private GameObject currentTouchedObject;

    private RaycastHit2D[] hitResults = new RaycastHit2D[10];

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
        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        touchPosition.z = 0;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                currentTouchedObject = GetTouchedObject(touchPosition);
                //Debug.Log("touch started");
                OnTouchBegan?.Invoke(touchPosition, touch);
                break;

            case TouchPhase.Moved:
                OnTouchMoved?.Invoke(touchPosition, touch);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                OnTouchEnded?.Invoke(touchPosition, touch);
                currentTouchedObject = null;
                //Debug.Log("Touch ended");
                break;
        }
    }

    public GameObject GetTouchedObject(Vector3 touchPosition)
    {
        int hitCount = Physics2D.RaycastNonAlloc(touchPosition, Vector2.zero, hitResults);

        if (hitCount <= 0) return null;

        return hitResults[0].collider?.gameObject;
    }

    public bool RaycastOnLayer(Vector3 position, LayerMask layerMask, out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, layerMask);
        return hit.collider != null;
    }

    public bool IsObjectTouched(GameObject obj, Vector3 touchPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == obj;
    }
}