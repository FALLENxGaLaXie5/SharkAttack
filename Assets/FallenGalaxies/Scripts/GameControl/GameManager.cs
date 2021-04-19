using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float spawnMultiplier = 0, spawnMultiplierIncrement = .1f;



    public int multiplierIncrements = 1000;
    int nextScoreIncremement;
    [SerializeField] int musicPitchIncrement = 500;
    int nextMusicPitchIncrement;
    [SerializeField] float pitchIncrement = 0.3f;
    [SerializeField] float pitchLerpSpeed = 0.05f;

    

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
        ScaleImagesToScreenSize();
        nextScoreIncremement = multiplierIncrements;
        nextMusicPitchIncrement = musicPitchIncrement;
        Scorer.instance.scoreIncreasedEvent.AddListener(ScoreUpdated);
    }

    void ScoreUpdated()
    {
        Debug.Log("ScoreUpdated Event Fired");
        if(Scorer.instance.currentScore > nextScoreIncremement)
        {
            spawnMultiplier += spawnMultiplierIncrement;
            if (spawnMultiplier > 0.95) spawnMultiplier = 0.95f;
            nextScoreIncremement += multiplierIncrements;
            Debug.Log("Multiplier increased to " + spawnMultiplier);
        }
        if (Scorer.instance.currentScore > nextMusicPitchIncrement)
        {
            //Tell timeline manager to play lerping sunlight affect
            TimelineManager.instance.PlayTimeline(0);
            nextMusicPitchIncrement += musicPitchIncrement;
            float currentPitch = MusicControl.instance.GetCurrentPitch("Track2");
            StartCoroutine(MusicControl.instance.LerpToNewPitch("Track2", currentPitch, currentPitch + pitchIncrement, pitchLerpSpeed));
        }
    }

    public static void UpdateScore(int amount)
    {
        //instance.currentScore += amount;
    }

    void ScaleImagesToScreenSize()
    {
        //Vector2 deviceScreenResolution = new Vector2(Screen.width, Screen.height);
        //Debug.Log(deviceScreenResolution);

        //Debug.Log(GameObject.Find("deep ocean").GetComponent<SpriteRenderer>().sprite.rect.width);
    }
}
