using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button levelTwoButton, levelThreeButton;
    private int levelPassed, sceneIndex;
    void Start()
    {
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
        levelTwoButton.interactable = false;
        levelThreeButton.interactable = false;

        switch (levelPassed)
        {
            case 3:
                levelTwoButton.interactable = true;
                break;
            case 5:
                levelTwoButton.interactable = true;
                levelThreeButton.interactable = true;
                break;
            default:
                Debug.Log("Not on a game level screen");
                break;
        }
    }
    public void resetPlayerPrefs()
    {
        levelTwoButton.interactable = false;
        levelThreeButton.interactable = false;
        PlayerPrefs.DeleteAll();
    }
}
