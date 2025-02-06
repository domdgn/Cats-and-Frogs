using System.Collections;
using UnityEngine;

public class CameraStartup : MonoBehaviour
{
    //best use LeanTween asset instead, much simpler
    //void Start()
    //{
    //    transform.position = new Vector3(0, -2, -10);
    //    LeanTween.moveY(gameObject, 0, 1f).setEaseInOutQuad();
    //}

    void Start()
    {
        transform.position = new Vector3(0, -2, -10);
        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        yield return new WaitForSecondsRealtime(3f);
        float time = 0;
        float duration = 1.5f;
        Vector3 start = transform.position;
        Vector3 end = new Vector3(0, 0, -10);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }
}