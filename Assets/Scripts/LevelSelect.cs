using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public SceneTransition sceneTransition;

    public GameObject[] mainMenu;
    public GameObject[] levelSelect;

    // Ordered array of buttons for loading levels.
    public GameObject[] levelButtons;

    void Awake()
    {
        int latestLevel = SaveProgress.Load();
        for (int i = 0; i < latestLevel + 1; i++)
        {
            if (i < levelButtons.Length)
            {
                levelButtons[i].GetComponent<Button>().interactable = true;
            }
            if (i < latestLevel)
            {
                levelButtons[i].transform.Find("Coin").gameObject.SetActive(true);
            }
        }
    }

    public void SelectLevel(int index)
    {
        sceneTransition.LoadWithTransition(index);
    }

    public void Back()
    {
        foreach (GameObject obj in mainMenu)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in levelSelect)
        {
            obj.SetActive(false);
        }
    }
}
