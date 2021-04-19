using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Scorer : MonoBehaviour
{
    TextMeshProUGUI tmp;
    public int currentScore, displayScore;
    public static Scorer instance;

    public UnityEvent scoreIncreasedEvent;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tmp = gameObject.GetComponent<TextMeshProUGUI>();

        if (scoreIncreasedEvent == null)
            scoreIncreasedEvent = new UnityEvent();
    }

    public static void UpdateScore(int amount)
    {
        instance.scoreIncreasedEvent.Invoke();
        Debug.Log("UpdateScore(" + amount + ")");
        instance.currentScore += amount;
        instance.StartCoroutine("IncreaseScore");
    }

    public IEnumerator IncreaseScore()
    {
        while (displayScore < currentScore)
        {
            displayScore++;
            tmp.text = displayScore.ToString("00000");
            yield return new WaitForSeconds(0.01f);
        }
    }

    public static int GetScore() { return instance.currentScore; }
    public static void SetScore(int newScore)
    {
        Debug.Log("SetScore");
        instance.currentScore = newScore;
        instance.tmp.text = newScore.ToString("00000");
    }

    void Test()
    {

    }
}
