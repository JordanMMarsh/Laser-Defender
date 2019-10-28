using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Level : MonoBehaviour {

    [SerializeField] float delayAfterDeath = 3f;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game Screen");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(DelayGameOver());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(delayAfterDeath);
        SceneManager.LoadScene("Game Over");
    }
}
