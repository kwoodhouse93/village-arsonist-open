using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
    public List<Transform> initialTargets;

    public float minSize;
    public float maxSize;
    // The distance between targets at which the camera will be zoomed all the way out.
    public float minZoomDistance;

    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;

    public float posSmoothTime;
    public float zoomSmoothTime;

    private Camera cam;
    private List<Transform> targets;
    private Vector3 posVel;
    private float zoomVel;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(leftLimit, -minSize, 0), new Vector3(leftLimit, minSize, 0));
        Gizmos.DrawLine(new Vector3(rightLimit, -minSize, 0), new Vector3(rightLimit, minSize, 0));
        Gizmos.DrawLine(new Vector3(-minSize, topLimit, 0), new Vector3(minSize, topLimit, 0));
        Gizmos.DrawLine(new Vector3(-minSize, bottomLimit, 0), new Vector3(minSize, bottomLimit, 0));
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        targets = initialTargets;
    }

    void LateUpdate()
    {
        targets.RemoveAll(IsNull);

        if (targets.Count == 0)
        {
            return;
        }

        Bounds bounds = GetTargetBounds();

        float xSize = bounds.size.x;
        float ySize = bounds.size.y * cam.aspect;
        float biggestSize = Mathf.Max(xSize, ySize);
        float targetSize = Mathf.Lerp(minSize, maxSize, biggestSize / minZoomDistance);
        cam.orthographicSize = Mathf.SmoothDamp(
            cam.orthographicSize,
            targetSize,
            ref zoomVel,
            zoomSmoothTime
        );

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float xPos = Mathf.Clamp(bounds.center.x, leftLimit+halfWidth, rightLimit-halfWidth);
        float yPos = Mathf.Clamp(bounds.center.y, bottomLimit+halfHeight, topLimit-halfHeight);

        cam.transform.position = Vector3.SmoothDamp(
            cam.transform.position,
            new Vector3(xPos, yPos, cam.transform.position.z),
            ref posVel,
            posSmoothTime
        );
    }

    public void SetTargets(List<Transform> newTargets)
    {
        targets.Clear();
        targets.AddRange(newTargets);
    }

    private Bounds GetTargetBounds()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (Transform t in targets)
        {
            if (t != null)
            {
                // Debug.DrawLine(t.position, t.position + new Vector3(0, 1f, 0), Color.red);
                bounds.Encapsulate(t.position);
            }
        }
        return bounds;
    }

    private static bool IsNull(Transform t)
    {
        return t == null;
    }
}
