using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidescrollCamera : MonoBehaviour
{
    public float scrollSpeed;
    public float leftLimit;
    public float rightLimit;

    void OnDrawGizmos()
    {
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(leftLimit, -cam.orthographicSize, 0), new Vector3(leftLimit, cam.orthographicSize, 0));
            Gizmos.DrawLine(new Vector3(rightLimit, -cam.orthographicSize, 0), new Vector3(rightLimit, cam.orthographicSize, 0));
        }
    }

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        Vector3 pos = transform.position;
        pos.x = leftLimit + camHalfWidth;
        transform.position = pos;
    }

    void FixedUpdate()
    {
        Camera cam = GetComponent<Camera>();
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        if (camHalfWidth * 2 > rightLimit - leftLimit)
        {
            Vector3 pos = transform.position;
            pos.x = ((rightLimit - leftLimit) / 2) + leftLimit;
            transform.position = pos;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 pos = transform.position;
            pos.x += scrollSpeed;
            if (pos.x <= rightLimit - camHalfWidth)
            {
                transform.position = pos;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 pos = transform.position;
            pos.x -= scrollSpeed;
            if (pos.x >= leftLimit + camHalfWidth)
            {
                transform.position = pos;
            }
        }
    }
}
