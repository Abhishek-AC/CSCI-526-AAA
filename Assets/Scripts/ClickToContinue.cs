using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToContinue : MonoBehaviour
{
    public void ContiueGame()
    {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
