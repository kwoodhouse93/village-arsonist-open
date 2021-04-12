#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[ExecuteInEditMode]
public class GeneratePrefabVariants : MonoBehaviour
{
    const int SPACING_CANDIDATES = 1;
    const int POINT_FINDER_LIMIT = 100;
    const float PIXEL_ALPHA_THRESHOLD = 0.1f;

    [SerializeField] private GameObject prefabToClone;
    [SerializeField] private string[] stripAssetNamePrefixes;
    [SerializeField] private Sprite[] spritesToGenerate;
    [SerializeField] private string outputPath;
    // Will only work if generating a single sprite.
    [SerializeField] private bool openPrefabAfterGen;

    //  Drawing info
    private Vector2? drawPoint;
    private Bounds? drawBounds;

    public void DrawRandomPoint()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        drawBounds = sr.bounds;

        Vector2 point = (Vector2)RandomPointWithinBounds(sr.bounds);
        drawPoint = point;
        SceneView.RepaintAll();
    }

    public void DrawRandomNonTransparentPoint()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        drawBounds = sr.bounds;

        Vector2 point = (Vector2)RandomNonTransparentPointWithinBounds(sr.bounds, sr, POINT_FINDER_LIMIT);
        drawPoint = point;
        SceneView.RepaintAll();
    }
    public void StopDrawing()
    {
        drawPoint = null;
        drawBounds = null;
        SceneView.RepaintAll();
    }

    void OnDrawGizmos()
    {
        if (drawPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere((Vector2)drawPoint, 0.05f);
        }
        if (drawBounds != null)
        {
            Gizmos.color = Color.blue;
            DrawBounds((Bounds)drawBounds);
        }
    }

    private void DrawBounds(Bounds bounds)
    {
        Vector2 bottomLeft = new Vector2(-bounds.extents.x, -bounds.extents.y);
        Vector2 bottomRight = new Vector2(bounds.extents.x, -bounds.extents.y);
        Vector2 topLeft = new Vector2(-bounds.extents.x, bounds.extents.y);
        Vector2 topRight = new Vector2(bounds.extents.x, bounds.extents.y);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    public void Generate()
    {
        foreach (Sprite s in spritesToGenerate)
        {
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefabToClone);

            try
            {
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = s;

                // Set center position to middle of sprite bounds
                Transform center = go.transform.Find("Center");
                center.localPosition = Vector3.zero + sr.bounds.center;

                // Set bars position to just below bottom of sprite
                Transform bars = go.transform.Find("Bars");
                bars.localPosition = sr.bounds.center + new Vector3(0, -sr.bounds.extents.y - 0.2f, 0);

                // Add physics collide and shadow caster. Fit shadow shape to polygon collider shape.
                go.AddComponent<PolygonCollider2D>();
                go.AddComponent<ShadowCaster2D>();
                ShadowCaster2DController scc = go.AddComponent<ShadowCaster2DController>();
                scc.UpdateFromCollider();
                DestroyImmediate(scc);

                // Reference fire effect children in BurnableObject script.
                BurnableObject bo = go.GetComponent<BurnableObject>();
                GameObject[] fireChildren = {
                    go.transform.GetChild(1).gameObject,
                    go.transform.GetChild(2).gameObject,
                    go.transform.GetChild(3).gameObject,
                };
                bo.fireObjects = fireChildren;

                // Set fire children positions to random but well distributed points
                // Based on Poisson-disc distribution explained here:
                // https://bost.ocks.org/mike/algorithms/
                List<Vector2> fcPositions = new List<Vector2>();
                foreach (GameObject fc in fireChildren)
                {
                    Vector2? point = GeneratePoint(fcPositions, sr, SPACING_CANDIDATES, POINT_FINDER_LIMIT);
                    if (point != null)
                    {
                        fc.transform.localPosition = (Vector2)point;
                    }
                    fcPositions.Add(fc.transform.localPosition);
                }

                string assetName = s.name;
                foreach (string prefix in stripAssetNamePrefixes)
                {
                    assetName = assetName.Replace(prefix, "");
                }
                assetName = Regex.Replace(assetName, @"\s+", "");
                string assetPath = outputPath + "/" + assetName + ".prefab";
                GameObject obj = PrefabUtility.SaveAsPrefabAsset(go, assetPath);
                if (openPrefabAfterGen && spritesToGenerate.Length == 1)
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(assetPath));
                }
            }
            finally
            {
                Object.DestroyImmediate(go);
            }

        }
    }

    private Vector2? GeneratePoint(
        List<Vector2> candidates,
        SpriteRenderer spriteRenderer,
        int spacingCandidates,
        int pointFinderLimit
    )
    {
        Vector2? bestCandidate = null;
        float bestDistance = 0;

        for (int i = 0; i < spacingCandidates; i++)
        {
            Vector2? point = RandomNonTransparentPointWithinBounds(spriteRenderer.bounds, spriteRenderer, pointFinderLimit);
            if (point == null)
            {
                continue;
            }

            Vector2 candidate = (Vector2)point;

            float distance = DistanceToClosest(candidates, candidate);
            if (distance > bestDistance)
            {
                bestDistance = distance;
                bestCandidate = candidate;
            }

            candidates.Add(candidate);
        }

        if (bestCandidate == null)
        {
            return null;
        }
        return bestCandidate;
    }

    private float DistanceToClosest(List<Vector2> candidates, Vector2 candidate)
    {
        // Nothing to compare against
        if (candidates.Count == 0)
        {
            return Mathf.Infinity;
        }

        float closestDistance = Mathf.Infinity;
        foreach (Vector2 c in candidates)
        {
            float dist = Vector2.Distance(c, candidate);
            if (dist < closestDistance)
            {
                closestDistance = dist;
            }
        }
        return closestDistance;
    }

    private Vector2? RandomNonTransparentPointWithinBounds(Bounds bounds, SpriteRenderer spriteRenderer, int attemptLimit)
    {
        for (int i = 0; i < attemptLimit; i++)
        {
            Vector2 point = RandomPointWithinBounds(bounds);
            Vector2Int pixel = PointToSpriteRendererPixel(point, spriteRenderer);
            float pixelAlpha = PixelAlpha(pixel, spriteRenderer);
            // Debug.Log("(" + point.x + "," + point.y + ")/(" + pixel.x + "," + pixel.y + ") " + pixelAlpha);
            if (pixelAlpha > PIXEL_ALPHA_THRESHOLD)
            {
                // Debug.Log("Took " + i + " attempts to find a non-transparent pixel");
                return point;
            }
        }
        return null;
    }

    private Vector2 RandomPointWithinBounds(Bounds bounds)
    {
        Vector2 pos = new Vector2(
            Random.Range(-bounds.extents.x, bounds.extents.x),
            Random.Range(-bounds.extents.y, bounds.extents.y)
        );
        return pos;
    }

    private Vector2Int ToInt2(Vector2 a)
    {
        return new Vector2Int((int)a.x, (int)a.y);
    }

    private Vector2Int PointToSpriteRendererPixel(Vector2 point, SpriteRenderer spriteRenderer)
    {
        Sprite s = spriteRenderer.sprite;

        float ppu = s.pixelsPerUnit;

        // Local position on the sprite in pixels.
        Vector2 localPos = point * ppu;
        // return new Vector2Int((int)localPos.x, (int)localPos.y);
        // return localPos;

        // When the sprite is part of an atlas, the rect defines its offset on the texture.
        // When the sprite is not part of an atlas, the rect is the same as the texture (x = 0, y = 0, width = tex.width, ...)
        var texSpacePivot = new Vector2(s.rect.x, s.rect.y) + s.pivot;
        Vector2 texSpaceCoord = texSpacePivot + localPos;
        return ToInt2(texSpaceCoord);
    }

    private float PixelAlpha(Vector2Int pixel, SpriteRenderer spriteRenderer)
    {
        Color pixelColor = spriteRenderer.sprite.texture.GetPixel(pixel.x, pixel.y);
        return pixelColor.a;
    }
}
#endif
