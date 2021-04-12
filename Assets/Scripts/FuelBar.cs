using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBar : MonoBehaviour
{
    public Burns burns;

    private float max;
    private float current;
    private float origX;

    // Start is called before the first frame update
    void Start()
    {
        max = burns.fuel;
        current = burns.fuel;
        origX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        current = burns.GetCurrentFuel();
        Scale();
        transform.rotation = Quaternion.identity;
    }

    private void Scale()
    {
        Vector3 s = transform.localScale;
        s.x = origX * (current / max);
        if (float.IsNaN(s.x))
        {
            s.x = 0;
        }
        transform.localScale = s;
    }
}
