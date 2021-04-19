using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public int score;
    public float[] position;

    public PlayerData(Player player)
    {
        this.level = player.GetLevel();
        this.health = player.GetHealth();
        this.score = player.GetScore();
        this.position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
    }

}
