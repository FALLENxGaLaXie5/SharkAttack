using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButton : MonoBehaviour
{

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

}
