using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TransitionSceneLevel : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] Animator sceneFader;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject warningField;
    [SerializeField] BetweenPlayerData betweenPlayerdata;

    public GameObject loadingScreen;
    public Slider slider;
    public GameObject textObject;

    Text tmp;

    void Start()
    {
        tmp = textObject.GetComponent<Text>();
    }

    public void LoadNextScene(int i)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsyncronously(i));
    }

    public void LoadSavedGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            betweenPlayerdata.SetNewGame(false);
            betweenPlayerdata.SetHealth(data.health);
            betweenPlayerdata.SetScore(data.score);
            betweenPlayerdata.SetLevel(data.level);
            betweenPlayerdata.SetPosition(new Vector2(data.position[0], data.position[1]));
            StartCoroutine(Transition(data.level));
        }
        else
        {
            Debug.LogWarning("No saved game found!");
            warningField.SetActive(true);
            StartCoroutine(DisappearWarning());
        }
    }

    IEnumerator LoadAsyncronously(int sceneIndex)
    {
        sceneFader.SetTrigger("transitionOut");
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        sceneFader.SetTrigger("transitionOut");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            tmp.text = progress.ToString("00%");
            yield return null;
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator Transition(int i)
    {
        sceneFader.SetTrigger("transitionOut");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(i);
    }

    IEnumerator TransitionAsyncronously(int i)
    {
        sceneFader.SetTrigger("transitionOut");
        yield return new WaitForSeconds(5f);
    }

    IEnumerator DisappearWarning()
    {
        yield return new WaitForSeconds(4f);
        warningField.SetActive(false);
    }

    public void PauseGame()
    {
        bool paused = playerControl.GetPaused();
        if (paused)
        {
            playerControl.SetPaused(false);
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        else
        {
            playerControl.SetPaused(true);
            Time.timeScale = 0;
            pauseMenu.SetActive(true);

        }
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        playerControl.SetPaused(!playerControl.GetPaused());
    }
}
