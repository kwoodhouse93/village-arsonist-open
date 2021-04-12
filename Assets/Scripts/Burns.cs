using UnityEngine;
using UnityEngine.Events;

public class Burns : MonoBehaviour
{
    // How close nearby fire has to be to have any effect.
    const float IGNITE_RADIUS = 4f;
    // Global modifier to use when applying flamability to temperature.
    const float FLAMABILITY_MODIFIER = 1.5f;

    // Determines how long the item can burn for.
    public float fuel;

    // Determines how hot the item has to be to fire to ignite.
    public float ignitability;
    // GameObject to identify center of object for checking nearby fire, if transform.position isn't it.
    public GameObject center;

    // Determines how hot the fire is, thus how quickly the item burns and how easily it will spread.
    public float flamability;

    // Audio source and flame audio clips to play.
    public AudioSource audioSource;
    public AudioClip[] flameAudioClips;
    public float minFlameVolume;
    public float maxFlameVolume;
    public float flameClipTimeJitter;

    // Is the object currently on fire?
    public bool onFire;

    // Event for count of all fuel consumed
    public UnityEvent<float> fuelConsumed;

    private bool ignited;
    private bool extinguished;
    private float currentFuel;
    private float currentTemp;

    private float nextSoundTime;

    private LayerMask flamableLayer;
    private Vector3 centerPos;

    private Ignitable toIgnite;
    private Destroyable toDestroy;
    private Extinguishable toExtinguish;

    // Start is called before the first frame update
    void Start()
    {
        currentFuel = fuel;

        toIgnite = GetComponent<Ignitable>();
        toDestroy = GetComponent<Destroyable>();
        toExtinguish = GetComponent<Extinguishable>();

        flamableLayer = LayerMask.GetMask("Object");

        nextSoundTime = 0;
    }

    void Update()
    {
        if (audioSource != null && flameAudioClips != null && flameAudioClips.Length > 0 && onFire && Time.time > nextSoundTime)
        {
            AudioClip clip = flameAudioClips[Random.Range(0, flameAudioClips.Length - 1)];
            audioSource.PlayOneShot(clip, Random.Range(minFlameVolume, maxFlameVolume));
            nextSoundTime = Time.time + clip.length + Random.Range(-flameClipTimeJitter, flameClipTimeJitter);
        }
    }

    void FixedUpdate()
    {
        UpdateCenterPos();
        UpdateTemperature();

        if (fuel != 0 && currentFuel <= 0)
        {
            Extinguish();
            return;
        }

        if (ignitability != 0 && currentTemp >= ignitability)
        {
            onFire = true;
        }

        if (onFire)
        {
            if (!ignited)
            {
                if (toIgnite != null)
                {
                    toIgnite.Ignite();
                }
                ignited = true;
            }
            float consumed = flamability * FLAMABILITY_MODIFIER;
            currentFuel -= consumed;
            fuelConsumed.Invoke(consumed);
        }
    }

    public void StopSFX()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            nextSoundTime = Time.time;
        }
    }

    public float GetCurrentFuel()
    {
        return currentFuel;
    }

    public float GetCurrentTemp()
    {
        return currentTemp;
    }

    public Vector2 GetCenterPos()
    {
        return centerPos;
    }

    private void UpdateCenterPos()
    {
        centerPos = transform.position;
        if (center != null)
        {
            centerPos = center.transform.position;
        }
    }

    private void UpdateTemperature()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(centerPos, IGNITE_RADIUS, flamableLayer);
        foreach (Collider2D col in hitColliders)
        {
            Burns burnable = col.gameObject.GetComponent<Burns>();
            if (burnable == null || !burnable.onFire)
            {
                continue;
            }

            ApplyTemperature(burnable);
        }
    }

    private void ApplyTemperature(Burns burnable)
    {
        float distance = Vector2.Distance(burnable.GetCenterPos(), transform.position);
        if (distance < 1) distance = 1;
        float intensity = 1 / Mathf.Pow(distance, 3f);
        currentTemp += intensity * burnable.flamability * FLAMABILITY_MODIFIER;
    }

    private void Extinguish()
    {
        StopSFX();
        onFire = false;
        if (!extinguished)
        {
            if (toDestroy != null)
            {
                toDestroy.Destroy();
            }
            if (toExtinguish != null)
            {
                toExtinguish.Extinguish();
            }
            extinguished = true;
        }
    }
}
