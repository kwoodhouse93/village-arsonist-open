using UnityEngine;

public class FireSource : MonoBehaviour, Ignitable, Extinguishable
{
    public GameObject[] fireObjects;

    public void Ignite()
    {
        foreach (GameObject fo in fireObjects)
        {
            fo.SetActive(true);
        }
    }

    public void Extinguish()
    {
        foreach (GameObject fo in fireObjects)
        {
            fo.SetActive(false);
        }
    }
}
