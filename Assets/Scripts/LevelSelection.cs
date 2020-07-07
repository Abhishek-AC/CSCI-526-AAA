using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button levelTwoButton, levelThreeButton;
    int levelPassed;
    void Start() {
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
        levelTwoButton.interactable = false;
        levelThreeButton.interactable = false;
        
        switch(levelPassed) {
            case 2: 
                levelTwoButton.interactable = true;
                break;
            case 4:
                levelTwoButton.interactable = true;
                levelThreeButton.interactable = true;
                break;
            default:
                Debug.Log("Not on a level scene");
                break;
        }
    }
    
    public void levelToLoad(int level) {
        SceneManager.LoadScene(level);
    }

    public void resetPlayerPrefs() {
        levelTwoButton.interactable = false;
        levelThreeButton.interactable = false;
        PlayerPrefs.DeleteAll();
    }
}
