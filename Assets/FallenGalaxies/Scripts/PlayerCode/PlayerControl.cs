using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour
{
    #region Inspector Control Variables
    [Header("General")]
    [Tooltip("Speed on X-axis")] [SerializeField] float xControlSpeed = 2f;
    [Tooltip("Speed on Y-axis")] [SerializeField] float yControlSpeed = 2f;

    [Space(10)]

    [Tooltip("Time between slings; will not let you slingshot before this time is up")] [Range(0.1f, 5.0f)] [SerializeField] float slingRechargeRate = 2f;

    [Tooltip("Maximum force that can be applied to shark")] [SerializeField] float maxForceToApply = 1000f;
    [Tooltip("Minimum force that can be applied to shark")] [SerializeField] float minForceToApply = 2f;

    [Tooltip("Speed control when you use touch input to slingshot")] [SerializeField] float slingSpeed = 2f;

    [Space(10)]

    [Tooltip("Speed of Rotation")] [SerializeField] float turningRate = 3f;
    [Tooltip("Will freeze velocity and make gravity scale 0 when screen is touched")] [SerializeField] bool freezeMovementOnTouch = true;
    [Tooltip("Will freeze slow down time for all objects when screen is touched")] [SerializeField] bool slowTimeOnTouch = true;
    [Tooltip("New Time Scale to apply when screen is touched; applied if slowTimeOnTouch is flagged")] [Range(0.1f, 0.9f)] [SerializeField] float slowedTimeScale = 0.5f;



    [Space(10)]

    [Header("Automatic Movement and Rotation Handles")]

    [Tooltip("Speed of Automatic Force Application")] [SerializeField] float automaticMovementRate = 3f;
    [Tooltip("Speed of Automatic Rotation")] [SerializeField] float automaticTurningRate = 3f;
    [Tooltip("Speed of Rotation in air")] [SerializeField] float maxAutomaticAirTurningRate = 3f;
    [Tooltip("Speed of Acceleration of Rotation in air to diving angle")] [SerializeField] float automaticAirTurningRateAcceleration = 0.2f;

    [Tooltip("Dive angle for shark; lower number for steeper dive angle")] [Range(0.01f, 0.9f)] [SerializeField] float airDiveAngle = 0.01f;
    [Tooltip("Controls how fast sling arrow indicator retracts back towards shark when player is not touching screen")] [Range(0.01f, 0.15f)] [SerializeField] float arrowRetractionSpeed = 0.07f;


    [Space(10)]

    [Header("Shark Rotation")]
    [Tooltip("Max Rotation Up")] [SerializeField] float maxRotationUp = 50f;
    //[Tooltip("Max Rotation Down")] [SerializeField] float maxRotationDown = 2f;

    [Space(10)]

    [Header("External Objects")]
    //[Tooltip("Object to look at when rotating in the air")] [SerializeField] Transform downRotationObject;
    [SerializeField] Transform slingHeadObject;
    [SerializeField] Transform slingTailObject;
    [SerializeField] Transform slingArrowObject;

    [Space(10)]

    [Header("External Objects")]

    [SerializeField] Transform slingMaskObject;
    [Tooltip("Controls how fast the arrow retracts")] [SerializeField] float maxTimeAtMaxSlingMagnitude = 2f;

    [SerializeField] Transform powerupMaskObject;
    [SerializeField] float maxPowerUpMaskScale = 2.11f;

    [SerializeField] float maxMagnitudeOfSlingVector = 130f;


    [Space(5)]
    #endregion

    #region Member/Private Variables
    Transform newTransform;
    AirMovement airMovementComponent;

    GameObject emptyPrefab;

    float xRange = 999f;
    float yRange = 999f;

    float newRotationOnZAxis;
    bool rightFACE;
    bool leftFACE;
    bool touchControlsEnabled = true;

    float xThrow, yThrow;
    bool controlsEnabled = true;
    bool paused = false;
    bool facingLeft = false;

    bool hasBeenSlung = false;

    float maxRotationDownRight;
    float maxRotationUpLeft;
    float maxRotationDownLeft;

    float currentAutomaticAirTurningRate = 0.0f;
    float startArrowXScale;


    Rigidbody2D rigidBody;

    Vector2 beginPos;
    Vector2 endPos;

    Vector2 currentVelocity;
    float currentGravityScale;

    float minMaskScale;
    float scaleXTotalDifference;

    float minMagnitudeOfSlingVector = 0;

    float currentMaxMagnitude;

    float currentMaxTimeTemp;
    float currentMaxTime;

    float currentMaxMaskScale;
    float currentMaxPowerupMaskScale;

    float maxMaskScale = 0f;
    float minTimeAtZeroSlingMagnitude = 0f;

    float differenceTotalForce;

    float minPowerupMaskScale;

    StaminaBar staminaBar;

    #endregion

    #region Unity Event Functions

    // Start is called before the first frame update
    void Start()
    {
        airMovementComponent = GetComponentInChildren<AirMovement>();
        staminaBar = GetComponentInChildren<StaminaBar>();

        Input.multiTouchEnabled = false;
        rigidBody = GetComponent<Rigidbody2D>();

        maxRotationDownRight = 360f - maxRotationUp;
        maxRotationUpLeft = 360f - maxRotationUp;
        maxRotationDownLeft = maxRotationUp;

        startArrowXScale = slingArrowObject.localScale.x;
        emptyPrefab = new GameObject();

        minMaskScale = slingMaskObject.localScale.x;
        scaleXTotalDifference = maxMaskScale - minMaskScale;

        currentMaxMagnitude = maxMagnitudeOfSlingVector;
        currentMaxTime = maxTimeAtMaxSlingMagnitude;
        currentMaxTimeTemp = currentMaxTime;
        currentMaxMaskScale = maxMaskScale;
        currentMaxPowerupMaskScale = maxPowerUpMaskScale;

        differenceTotalForce = maxForceToApply - minForceToApply;

        minPowerupMaskScale = powerupMaskObject.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            ProcessMovement();
        }
    }

    void FixedUpdate()
    {
        ProcessAutomaticTranslation();
    }

    #endregion

    #region Class Functions
    void ProcessMovement()
    {
        if (controlsEnabled)
        {
            //keyboard input for PC testing
            ProcessTranslationPC();
        }

        //all automatic rotation in water and air is done in here
        ProcessAutomaticRotation();

        if (touchControlsEnabled)
        {
            //any rotation code as a result of moving fingers on a touch screen, along with slingshot physics is done in DetectTouch
            DetectTouch();
        }

        ProcessReturnOfSlingArrow();
    }

    void ProcessReturnOfSlingArrow()
    {
        if (Input.touchCount <= 0)
        {
            currentMaxTimeTemp += Time.deltaTime;
            float percRatio = currentMaxTimeTemp / currentMaxTime;


            slingMaskObject.localScale = new Vector3(Mathf.Lerp(currentMaxMaskScale, minMaskScale, percRatio), slingMaskObject.localScale.y, slingMaskObject.localScale.z);
            powerupMaskObject.localScale = new Vector3(Mathf.Lerp(currentMaxPowerupMaskScale, minPowerupMaskScale, percRatio), powerupMaskObject.localScale.y, powerupMaskObject.localScale.z);

        }
    }

    //calculates force to be applied to player automatically for smooth forward movement all the time
    void ProcessAutomaticTranslation()
    {
        float xOffset;
        if (facingLeft)
        {
            xOffset = -automaticMovementRate * Time.fixedDeltaTime;
        }
        else
        {
            xOffset = automaticMovementRate * Time.fixedDeltaTime;
        }

        float clampedXPos = Mathf.Clamp(xOffset + transform.position.x, -xRange, xRange);

        rigidBody.AddForce(transform.right.normalized * xOffset);
    }

    //All rotation to a level plane in the water, and automatic rotation to dive back into the water in the air, is done in here
    void ProcessAutomaticRotation()
    {
        if (airMovementComponent.GetInAir())
        {
            DiveFromAirRotation();
        }
        else
        {
            LevelFishInWater();
        }
    }

    void DiveFromAirRotation()
    {
        Vector2 downPoint = new Vector2(this.transform.position.x, this.transform.position.y - 100f);
        Vector2 lookVector;
        if (!facingLeft)
        {
            lookVector = new Vector2(airDiveAngle, -1.0f);
        }
        else
        {
            lookVector = new Vector2(-airDiveAngle, -1.0f);
        }

        if (airMovementComponent.GetInAir() && currentAutomaticAirTurningRate < maxAutomaticAirTurningRate)
        {
            currentAutomaticAirTurningRate += (automaticAirTurningRateAcceleration * Time.deltaTime);
        }
        LookAtTargetAir(lookVector, currentAutomaticAirTurningRate);
    }

    void LevelFishInWater()
    {
        Quaternion rotation = Quaternion.identity;
        Quaternion lerpedRotation = Quaternion.Lerp(transform.rotation, rotation, automaticTurningRate * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, lerpedRotation.eulerAngles.z);
    }

    void ProcessTranslationPC()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * xControlSpeed * Time.deltaTime;
        float clampedXPos = Mathf.Clamp(xOffset + transform.position.x, -xRange, xRange);

        float yOffset = yThrow * yControlSpeed * Time.deltaTime;
        float clampedYPos = Mathf.Clamp(yOffset + transform.position.y, -yRange, yRange);

        transform.position = new Vector2(clampedXPos, clampedYPos);
    }

    void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            ProcessTranslationMobile();
            CheckForScreenTouch();
        }
        else
        {
            controlsEnabled = true;
        }
    }

    void ProcessTranslationMobile()
    {
        if (Input.touches[0].phase == TouchPhase.Began)
        {
            staminaBar.SetRecharging(false);
            beginPos = Input.touches[0].position;
            controlsEnabled = false;
            currentMaxMagnitude = maxMagnitudeOfSlingVector;
            currentMaxTime = maxTimeAtMaxSlingMagnitude;
            currentMaxTimeTemp = currentMaxTime;
        }
        if (Input.touches[0].phase == TouchPhase.Moved)
        {
            //will just process sling vector to calculate if player can rotate on it
            ProcessVectorAndRotation();
        }
        if (Input.touches[0].phase == TouchPhase.Ended)
        {
            StartCoroutine(staminaBar.StartRecharging());
            ProcessSlingshot();
            controlsEnabled = true;
        }
    }

    //will be called not when finger is lifted; only when finger moved
    void ProcessVectorAndRotation()
    {
        Vector2 slingVector = GetCurrentSlingVector();
        rightFACE = !facingLeft && CheckDirection(slingVector);
        leftFACE = facingLeft && CheckDirection(slingVector);
        if (rightFACE)
        {
            LookAtTarget(slingVector, turningRate);

        }
        else if (leftFACE)
        {
            LookAtTarget(-slingVector, turningRate);
        }

    }

    void CheckForScreenTouch()
    {
        if (slowTimeOnTouch)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                Time.timeScale = slowedTimeScale;
                staminaBar.SetRecharging(false);
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                Time.timeScale = 1.0f;
            }
        }
        if (freezeMovementOnTouch)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                currentVelocity = rigidBody.velocity;
                rigidBody.velocity = Vector2.zero;
                currentGravityScale = rigidBody.gravityScale;
                rigidBody.gravityScale = 0f;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                rigidBody.gravityScale = currentGravityScale;
            }
        }
    }

    void ProcessVectorAndRotationAir()
    {
        Vector2 slingVector = GetCurrentSlingVectorAir();
        bool myRightFACE = !facingLeft && CheckDirection(slingVector);
        bool myLeftFACE = facingLeft && CheckDirection(slingVector);
        if (myRightFACE)
        {
            LookAtTargetAir(slingVector, maxAutomaticAirTurningRate);

        }
        else if (myLeftFACE)
        {
            LookAtTargetAir(-slingVector, maxAutomaticAirTurningRate);
        }

    }

    //will actually physically rotate shark, along with lerp the arrow forward
    void LookAtTarget(Vector2 slingVector, float turnRate)
    {

        Quaternion rotation = Quaternion.LookRotation(slingVector);
        Quaternion lerpedRotation = Quaternion.Lerp(transform.rotation, rotation, turnRate * Time.deltaTime);

        if (CheckLerpedRotation())
        {
            LerpArrowForward(slingVector);
            transform.rotation = Quaternion.Euler(0, 0, lerpedRotation.eulerAngles.z);
        }
    }

    void LerpArrowForward(Vector2 slingVector)
    {
        float perc = Mathf.Abs(slingVector.magnitude) / maxMagnitudeOfSlingVector;
        slingMaskObject.localScale = new Vector3(Mathf.Lerp(minMaskScale, maxMaskScale, perc), slingMaskObject.localScale.y, slingMaskObject.localScale.z);
        powerupMaskObject.localScale = new Vector3(Mathf.Lerp(minPowerupMaskScale, maxPowerUpMaskScale, perc), powerupMaskObject.localScale.y, powerupMaskObject.localScale.z);
    }

    void LookAtTargetAir(Vector2 slingVector, float turnRate)
    {
        Quaternion rotation = Quaternion.LookRotation(slingVector);
        Quaternion lerpedRotation = Quaternion.Lerp(transform.rotation, rotation, turnRate * Time.deltaTime);

        if (CheckLerpedRotation())
        {
            transform.rotation = Quaternion.Euler(0, 0, lerpedRotation.eulerAngles.z);
        }
    }

    bool CheckLerpedRotation()
    {

        return true;
    }

    //only called when finger comes off screen
    void ProcessSlingshot()
    {
        Vector2 slingVector = GetCurrentSlingVector();
        Vector2 slingVectorThrow = GetVectorToThrow();

        if (staminaBar.GetCurrentStamina() > 0 && !hasBeenSlung)
        {
            Vector3 forceAdded = slingVectorThrow.normalized * slingVector.magnitude * slingSpeed;
            float forceAddedMagnitudeClamped = Mathf.Clamp(forceAdded.magnitude, minForceToApply, maxForceToApply);

            //calculates total force to apply, taking into account the magnitude the player requested, along with current stamina
            float forceCalculationWithCurrentStamina = GetClampedForceToApply(forceAddedMagnitudeClamped);

            rigidBody.AddForce(forceAdded.normalized * forceCalculationWithCurrentStamina);
            hasBeenSlung = true;
            StartCoroutine(WaitForNextSling());
        }

        //set currentMaxMagnitude to the lastSlingVector's magnitude
        ProcessReturnOfArrowData(slingVector);
    }

    float GetClampedForceToApply(float forceAddedMagnitudeClamped)
    {
        float staminaPercUsed = forceAddedMagnitudeClamped / differenceTotalForce;
        float oldStaminaCache = staminaBar.GetPercentStamina();
        float newStaminaPerc = Mathf.Clamp(oldStaminaCache - staminaPercUsed, 0, 1);
        staminaBar.SetPercentStamina(newStaminaPerc);
        staminaBar.SetCurrentStamina(staminaBar.GetMaxStamina() * newStaminaPerc);

        float forceCalculationWithCurrentStamina = Mathf.Clamp(oldStaminaCache * differenceTotalForce, minForceToApply, forceAddedMagnitudeClamped);
        return forceCalculationWithCurrentStamina;
    }

    void ProcessReturnOfArrowData(Vector2 slingVector)
    {
        currentMaxMagnitude = Mathf.Clamp(slingVector.magnitude, minMagnitudeOfSlingVector, maxMagnitudeOfSlingVector);
        float perc = currentMaxMagnitude / maxMagnitudeOfSlingVector;
        currentMaxTime = perc * maxTimeAtMaxSlingMagnitude;
        currentMaxMaskScale = (1f - perc) * (maxMaskScale + minMaskScale);
        currentMaxPowerupMaskScale = perc * (maxPowerUpMaskScale - minPowerupMaskScale);
        print("CurrentMaxPowerupMaskScale: " + currentMaxPowerupMaskScale);
        currentMaxTimeTemp = 0f;
    }

    Vector2 GetCurrentSlingVector()
    {
        endPos = Input.touches[0].position;
        Vector2 slingVector = beginPos - endPos;
        //slingArrowObject.localScale = new Vector3((slingVector.magnitude * 0.01f) + startArrowXScale, slingArrowObject.localScale.y, slingArrowObject.localScale.z);
        return slingVector;
    }

    Vector2 GetVectorToThrow()
    {
        return slingHeadObject.position - slingTailObject.position;
    }

    Vector2 GetCurrentSlingVectorAir()
    {
        Vector2 downPoint = new Vector2(this.transform.position.x, this.transform.position.y - 100f);
        Vector2 lookVector = downPoint - (new Vector2(this.transform.position.x, this.transform.position.y));
        return lookVector;
    }

    bool CheckDirection(Vector2 slingVector)
    {
        if (facingLeft && slingVector.x < 0)
        {
            emptyPrefab.transform.right = -slingVector;
            return CheckNewRotation(emptyPrefab.transform);
        }
        else if (!facingLeft && slingVector.x > 0)
        {
            emptyPrefab.transform.right = slingVector;
            return CheckNewRotation(emptyPrefab.transform);
        }
        return false;

    }

    bool CheckNewRotation(Transform newRotationToCheck)
    {
        newRotationOnZAxis = newRotationToCheck.localEulerAngles.z;
        if (facingLeft)
        {
            return newRotationOnZAxis > maxRotationUpLeft || newRotationOnZAxis < maxRotationDownLeft;
        }
        else
        {
            return newRotationOnZAxis > maxRotationDownRight || newRotationOnZAxis < maxRotationUp;
        }
    }

    IEnumerator WaitForNextSling()
    {
        yield return new WaitForSeconds(slingRechargeRate);
        hasBeenSlung = false;
    }

    #endregion

    #region Getters/Setters

    public bool GetTouchControlsEnabled()
    {
        return this.touchControlsEnabled;
    }

    public void SetTouchControlsEnabled(bool newTouchControlsEnabled) { this.touchControlsEnabled = newTouchControlsEnabled; }

    public bool GetFacingLeft() { return this.facingLeft; }
    public void SetFacingLeft(bool newFacingLeft) { this.facingLeft = newFacingLeft; }
    public void ResetAutomaticAirTurningRate() { this.currentAutomaticAirTurningRate = 0.0f; }

    public void SetPaused(bool newPaused) { this.paused = newPaused; }
    public bool GetPaused() { return this.paused; }


    #endregion
}