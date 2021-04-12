using UnityEngine;

public class BurnableObject : MonoBehaviour, Ignitable, Destroyable
{
    public GameObject[] fireObjects;
    public GameObject destroyed;

    public void Ignite()
    {
        foreach (GameObject fo in fireObjects)
        {
            fo.SetActive(true);
        }
    }

    public void Destroy()
    {
        Object.Instantiate(destroyed, transform.position, transform.rotation);
        Object.Destroy(gameObject);
    }
}
