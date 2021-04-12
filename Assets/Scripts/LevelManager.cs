using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    const float WIN_MENU_DELAY = 3f;
    const float LOSE_MENU_DELAY = 3f;

    public ProgressBar progressBar;
    public AudioSource musicAudioSource;
    public AudioSource[] sfxAudioSources;
    public AudioClip preDropMusic;
    public AudioClip postDropMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    public AudioClip goalSFX;

    public float goal;

    public SceneTransition sceneTransition;

    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject pauseMenu;

    public int menuScene;

    public MultiTargetCamera targetCam;

    public UnityEvent paused;
    public UnityEvent unpaused;

    private Burns[] burnableObjects;
    private bool torchDropped;
    private float maxBurnable;
    private float totalBurnt;
    private bool triggeredWin;
    private bool triggeredLoss;

    private bool menuOpen;

    void Start()
    {
        musicAudioSource.clip = preDropMusic;
        musicAudioSource.Play();

        maxBurnable = 0;
        burnableObjects = Object.FindObjectsOfType<Burns>();
        foreach (Burns bo in burnableObjects)
        {
            if (bo.GetComponent<PlayerTorch>() == null)
            {
                bo.fuelConsumed.AddListener(OnFuelConsumed);
                maxBurnable += bo.fuel;
            }
        }

        progressBar.SetCurrentGoal(goal);
    }

    void Update()
    {
        if (triggeredWin || triggeredLoss)
        {
            return;
        }
        if ((totalBurnt / maxBurnable) > goal)
        {
            triggeredWin = true;
            progressBar.TriggerGoalAnimation();
            sfxAudioSources[0].PlayOneShot(goalSFX, 1f);
            StartCoroutine(PopWinMenu());
        }

        if (torchDropped)
        {
            List<Transform> burning = new List<Transform>();
            foreach (Burns bo in burnableObjects)
            {
                if (bo != null && bo.onFire)
                {
                    burning.Add(bo.transform);
                }
            }
            if (targetCam != null)
            {
                targetCam.SetTargets(burning);
            }
            if (burning.Count < 1)
            {
                triggeredLoss = true;
                StartCoroutine(PopLoseMenu());
            }
        }
    }

    IEnumerator PopWinMenu()
    {
        // NOTE: Assumes build index is equal to level number.
        SaveProgress.Save(SceneManager.GetActiveScene().buildIndex);

        yield return new WaitForSeconds(WIN_MENU_DELAY);
        menuOpen = true;
        foreach (Burns bo in burnableObjects)
        {
            bo.StopSFX();
        }
        foreach (AudioSource a in sfxAudioSources)
        {
            a.Pause();
        }
        musicAudioSource.Stop();
        musicAudioSource.clip = winMusic;
        musicAudioSource.Play();
        winMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    IEnumerator PopLoseMenu()
    {
        yield return new WaitForSeconds(LOSE_MENU_DELAY);
        menuOpen = true;
        foreach (Burns bo in burnableObjects)
        {
            bo.StopSFX();
        }
        foreach (AudioSource a in sfxAudioSources)
        {
            a.Pause();
        }
        musicAudioSource.Stop();
        musicAudioSource.clip = loseMusic;
        musicAudioSource.Play();
        loseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnDrop()
    {
        torchDropped = true;
        musicAudioSource.Stop();
        musicAudioSource.clip = postDropMusic;
        musicAudioSource.Play();
    }

    public void OnFuelConsumed(float amount)
    {
        totalBurnt += amount;
        if (totalBurnt > maxBurnable)
        {
            totalBurnt = maxBurnable;
        }
        progressBar.SetCurrentValue(totalBurnt / maxBurnable);
    }

    public void Pause()
    {
        if (!menuOpen)
        {
            paused.Invoke();
            Time.timeScale = 0f;
            musicAudioSource.Pause();
            foreach (Burns bo in burnableObjects)
            {
                bo.StopSFX();
            }
            foreach (AudioSource a in sfxAudioSources)
            {
                a.Pause();
            }
            pauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        unpaused.Invoke();
        Time.timeScale = 1f;
        musicAudioSource.Play();
        foreach (AudioSource a in sfxAudioSources)
        {
            a.Play();
        }
        pauseMenu.SetActive(false);
    }

    public void Reset()
    {
        Time.timeScale = 1f;
        sceneTransition.LoadWithTransition(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        sceneTransition.LoadWithTransition(menuScene);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        sceneTransition.LoadWithTransition(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
