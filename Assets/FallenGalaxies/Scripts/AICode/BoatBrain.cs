using Assets.Dasbor.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBrain : MonoBehaviour, IBrain
{

    #region Public/Serialized/Inspector Variables
    public Vector3 direction;
    public float speed = 2f;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float rateOfChange = 0.5f;
    [SerializeField] float totalSpeedChangeTime = 4f;
    public float currentSpeed;
    #endregion


    #region Private Variables
    float startTime;
    float totalSpeedDifference;
    float timePercentage = 0f;
    float defaultSpeed;
    bool sighted = false;
    #endregion

    void Start()
    {
        startTime = 0f;
        totalSpeedDifference = maxSpeed - speed;

        defaultSpeed = speed;
        currentSpeed = speed;
        direction = new Vector3(1f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        BoatMovement();
    }

    void BoatMovement()
    {
        if (sighted)
        {
            startTime += Time.deltaTime;
            float journeyFraction = startTime / totalSpeedChangeTime;
            currentSpeed = Mathf.Lerp(speed, maxSpeed, journeyFraction);
        }
        else
        {
            startTime += Time.deltaTime;
            float journeyFraction = startTime / totalSpeedChangeTime;
            currentSpeed = Mathf.Lerp(maxSpeed, speed, journeyFraction);
        }

        transform.parent.position = Vector2.MoveTowards(transform.position, transform.position + direction, currentSpeed * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
        if (direction.x < 0 && transform.parent.localScale.x > 0)
        {
            transform.parent.localScale = new Vector3(-transform.parent.localScale.x, transform.parent.localScale.y, transform.parent.localScale.z);
        }
        else if (direction.x > 0 && transform.parent.localScale.x < 0)
        {
            transform.parent.localScale = new Vector3(transform.parent.localScale.x * -1, transform.parent.localScale.y, transform.parent.localScale.z);
        }
    }

    #region Getters and Setters
    public Vector2 GetDirection()
    {
        return direction;
    }

    public float GetDefaultSpeed()
    {
        return this.defaultSpeed;
    }

    public void SetSighted(bool newSighted)
    {
        this.sighted = newSighted;
    }

    public bool GetSighted()
    {
        return this.sighted;
    }

    public void SetStartTime(float newTimer)
    {
        this.startTime = newTimer;
    }

    public float GetTimer()
    {
        return this.startTime;
    }

    public float GetCurrentSpeed()
    {
        return this.currentSpeed;
    }

    public float GetMinSpeed()
    {
        return this.speed;
    }

    public float GetMaxSpeed()
    {
        return this.maxSpeed;
    }

    public float GetTotalSpeedDifference()
    {
        return this.totalSpeedDifference;
    }

    public float GetTotalSpeedChangeTime()
    {
        return this.totalSpeedChangeTime;
    }
    #endregion
}
