using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButtonModified : MonoBehaviour
{

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

}
