using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject[] levelSelect;
    public GameObject[] mainMenu;

    public void Play()
    {
        foreach (GameObject obj in levelSelect)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in mainMenu)
        {
            obj.SetActive(false);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
