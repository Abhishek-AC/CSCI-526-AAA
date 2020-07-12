using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button levelTwoButton, levelThreeButton;
    private int levelPassed, sceneIndex;
    /*
    Feature removed as player can be stuck on a level 
    and get frustated so all levels should be accessible to the player
    
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
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(Wait());
        levelTwoButton.interactable = false;
        levelThreeButton.interactable = false;
        PlayerPrefs.DeleteAll();
    }
    */
    

    //SFX++
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        /*
        levelTwoButton.interactable = false;
        levelThreeButton.interactable = false;
        PlayerPrefs.DeleteAll();
        */
    }
}
