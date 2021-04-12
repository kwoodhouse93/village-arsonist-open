using System.Collections;
using UnityEngine;

public class ScaleWithCamera : MonoBehaviour
{
    public Camera cam;
    public float defaultOrthSize;
    public float scaleFactor;

    private Vector3 initScale;

    void Awake()
    {
        initScale = transform.localScale;
    }

    void LateUpdate()
    {
        Vector3 newScale = initScale * (cam.orthographicSize / defaultOrthSize) * scaleFactor;
        if (!float.IsNaN(newScale.x) && !float.IsNaN(newScale.y) && !float.IsNaN(newScale.z))
        {
            transform.localScale = newScale;
        }
    }
}
