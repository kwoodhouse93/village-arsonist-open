using UnityEngine;

public class JiggleUpAndDown : MonoBehaviour
{
    public float maxJiggle;
    public float timeScaler;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float posChange = Mathf.Sin(Time.time * timeScaler) * Mathf.Cos(Time.time * timeScaler * 3.14f) * maxJiggle;
        transform.position = new Vector2(transform.position.x, startY + posChange);
    }
}
