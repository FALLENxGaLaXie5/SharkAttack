using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region PlayerStatsVariables
    int level = 1;
    int health = 10;
    int score = 0;
    #endregion

    BetweenPlayerData theInbetweenObject;
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("TheInBetween"))
        {
            theInbetweenObject = GameObject.FindGameObjectWithTag("TheInBetween").transform.GetComponent<BetweenPlayerData>();
            LoadGame();
        }        
    }

    void LoadGame()
    {
        if (!theInbetweenObject.GetNewGame())
        {
            ApplyLoadedData(theInbetweenObject.GetLevel(), theInbetweenObject.GetHealth(), theInbetweenObject.GetScore(), theInbetweenObject.GetPosition());
        }
    }

    public void SavePlayer()
    {
        UpdateCurrentStats();
        SaveSystem.SavePlayer(this);
    }

    void UpdateCurrentStats()
    {
        this.level = SceneManager.GetActiveScene().buildIndex;
        this.health = 10;
        this.score = Scorer.GetScore();
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        this.level = data.level;
        this.health = data.health;
        this.score = data.score;

        Vector2 position = new Vector2(data.position[0], data.position[1]);
        ApplyLoadedData(this.level, this.health, this.score, position);
    }

    void ApplyLoadedData(int newLevel, int newHealth, int newScore, Vector2 newPosition)
    {
        //load the new scene if a different scene
        if (newLevel != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(newLevel);
            //TODO: need to load the player position, health, etc into the new scene as well. Use don't destroy on load object if exists
        }
        else
        {
            //TODO: apply player health to where it's stored
            this.transform.position = newPosition;
            Scorer.SetScore(newScore);
        }
    }

    #region Get/Setter Functions
    public int GetLevel() { return this.level; }
    public void SetLevel(int newLevel) { this.level = newLevel; }
    public int GetHealth() { return this.health; }
    public void SetHealth(int newHealth) { this.health = newHealth; }
    public int GetScore() { return this.score; }
    public void SetScore(int newScore) { this.score = newScore; }
    #endregion

}
