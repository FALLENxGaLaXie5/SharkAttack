using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenPlayerData : MonoBehaviour
{
    bool newGame = true;
    int health = 0;
    int score = 0;
    int level = 1;
    Vector2 position = Vector2.zero;

    #region Get/Setter Functions
    public bool GetNewGame() { return this.newGame; }
    public void SetNewGame(bool newNewGame) { this.newGame = newNewGame; }
    public int GetHealth() { return this.health; }
    public void SetHealth(int newHealth) { this.health = newHealth; }
    public int GetScore() { return this.score; }
    public void SetScore(int newScore) { this.score = newScore; }
    public Vector2 GetPosition() { return this.position;  }
    public void SetPosition(Vector2 newPosition) { this.position = newPosition; }

    public int GetLevel() { return this.level; }
    public void SetLevel(int newLevel) { this.level = newLevel; }
    #endregion
}
