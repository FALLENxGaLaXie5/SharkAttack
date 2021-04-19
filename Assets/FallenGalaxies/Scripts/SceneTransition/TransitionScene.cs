using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TransitionScene : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] Animator sceneFader;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject warningField;
    [SerializeField] BetweenPlayerData betweenPlayerdata;
    [SerializeField] int mainMenuScene = 0;
    [SerializeField] int level1Scene = 1;
    [SerializeField] float timeToWaitBetweenMusicFadeIn = 3f;


    public GameObject loadingScreen;
    public Slider slider;
    public GameObject textObject;

    Text tmp;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);        
    }
    void Start()
    {
        tmp = textObject.GetComponent<Text>();
    }

    public void LoadLevelSceneFromMenu(int track)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsyncronously(1));
        FindObjectOfType<MusicControl>().FadeOut("Track" + track);
    }


    public void LoadMainMenuScene(int track)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsyncronously(0));
        FindObjectOfType<MusicControl>().FadeOut("Track" + track);
    }
    
    public void LoadNextSceneNonAsynchronous(int i)
    {
        Time.timeScale = 1;
        StartCoroutine(Transition(i));
    }

    public void LoadMainMenuNonAsynchronous(int trackFadeOut, int trackFadeIn)
    {
        Time.timeScale = 1;
        StartCoroutine(Transition(0));
        FindObjectOfType<MusicControl>().FadeOut("Track" + trackFadeOut);
        StartCoroutine(WaitForMusicBetweenScene(timeToWaitBetweenMusicFadeIn, trackFadeIn));
        BeginFadeIn();
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
        if (mainMenu) { mainMenu.SetActive(false); }
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        //sceneFader.SetTrigger("transitionOut");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            tmp.text = progress.ToString("00%");
            yield return null;
        }
    }

    IEnumerator WaitForMusicBetweenScene(float waitTime, int trackFadeIn)
    {
        yield return new WaitForSeconds(waitTime);
        FindObjectOfType<MusicControl>().FadeIn("Track" + trackFadeIn);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator Transition(int i)
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
    
    public void BeginFadeIn()
    {
        sceneFader.SetTrigger("transitionIn");
    }
}
