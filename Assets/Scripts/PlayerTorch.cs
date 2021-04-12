using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTorch : MonoBehaviour
{
    const float UNPAUSE_DELAY = 0.1f;
    const float CURSOR_FOLLOW_SPEED = 0.1f;

    public Burns burns;
    public GameObject aimLight;

    public UnityEvent torchDrop;

    private bool dropped;
    private bool paused;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        dropped = false;
        paused = false;
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
    }

    public void OnPaused()
    {
        paused = true;
    }

    public void OnUnpaused()
    {
        StartCoroutine(DelayUnpause());
    }

    IEnumerator DelayUnpause()
    {
        yield return new WaitForSeconds(UNPAUSE_DELAY);
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dropped && !paused)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePos, CURSOR_FOLLOW_SPEED);
            if (Input.GetMouseButtonUp(0))
            {
                dropped = true;
                burns.onFire = true;
                rb.simulated = true;
                if (aimLight != null)
                {
                    Object.Destroy(aimLight);
                }
                torchDrop.Invoke();
            }
        }
    }
}
