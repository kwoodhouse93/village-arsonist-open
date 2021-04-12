using System.Collections;
using UnityEngine;

public class AttachToScreenLeft : MonoBehaviour
{
    public float width;
    public Camera cam;

    void LateUpdate()
    {
        Vector3 pos = transform.localPosition;
        pos.x = -cam.orthographicSize * cam.aspect + (width*transform.localScale.x / 2);
        transform.localPosition = pos;
    }
}
