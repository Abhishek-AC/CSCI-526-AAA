using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public void Quit () {
        Debug.Log("End Game in Build Mode");
        Application.Quit();
    }
    public void Relay() {
        Debug.Log("Replay");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
}
