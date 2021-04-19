using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoaderBar : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider slider;
    public GameObject textMeshProObject;

    TextMeshProUGUI text;

    void Start()
    {
        text = textMeshProObject.GetComponent<TextMeshProUGUI>();
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsyncronously(sceneIndex));        
    }

    IEnumerator LoadAsyncronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            text.text = progress + "%";
            yield return null;
        }
    }

}
