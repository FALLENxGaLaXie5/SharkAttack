using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupEffects : MonoBehaviour
{
    #region Inspector Control Variables
    [Header("Base Material Inspector Variable")]
    [SerializeField] Material baseMaterial;

    [Header("Health Powerup Inspector Variables")]
    [SerializeField] Material healthPowerupMaterial;
    [SerializeField] float minHealthPowerupAlpha = 0f;
    [SerializeField] float maxHealthPowerupAlpha = 240f;
    [SerializeField] float speedHealthPowerup = 20f;
    [SerializeField] float speedHealthPowerdown = 10f;
    [Space(5)]

    [Header("Shield Powerup Inspector Variables")]
    [SerializeField] Material bodyShieldPowerupMaterial;
    [SerializeField] Material headShieldPowerupMaterial;
    [SerializeField] float minShieldValue = 1.0f;
    [SerializeField] float maxShieldValue = 3.0f;
    private float noFadeShieldValue = 0.0f;
    //shield will not show at a fade value of 1. Increase and modulate between 1 and 3, then go back to 0.
    [SerializeField] float shieldEffectSpeed = 1.2f;
    [SerializeField] float shieldPowerupTimeInEffect = 3f;
    public bool currentlyFadingShield = false;
    public float currentFadeShield;
    public bool shieldActivate = false;


    [Space(5)]

    [Header("Stamina Powerup Inspector Variables")]
    [SerializeField] Material staminaPowerupMaterial;

    [SerializeField] float staminaEffectSpeed = 1f;
    [SerializeField] float staminaPowerupTimeInEffect = 3f;
    //stamina will not show at a fade value of 1. Decrease and modulate, then go back to 1 w                                                                                                                                      hen finished
    [SerializeField] float noFadeStamina = 1f;
    [SerializeField] float lowFadeStamina = 0.38f;
    [SerializeField] float highFadeStamina = 0.65f;
    


    [Space(5)]


    #endregion

    #region Components Not Set Until Start
    public Material currentMaterialBody;
    public Material currentMaterialHead;

    SpriteRenderer bodyRenderer;
    SpriteRenderer headRenderer;
    PowerupImplementations powerupImpl;

    #endregion

    #region Class Variables
    bool healthActivate = false;
    bool staminaActivate = false;


    bool currentlyIncreasingHealth = true;
    float currentFadeHealth;

    bool currentlyFadingStamina = true;
    float currentFadeStamina;

    bool powerupActive = false;
    bool healthActive = false;
    bool staminaActive = false;
    bool shieldActive = false;
    #endregion

    #region Unity Specific Events

    // Start is called before the first frame update
    void Start()
    {
        currentFadeStamina = noFadeStamina;
        currentFadeHealth = minHealthPowerupAlpha;
        currentFadeShield = noFadeShieldValue;
        bodyRenderer = GetComponent<SpriteRenderer>();
        headRenderer = transform.Find("Head").GetComponent<SpriteRenderer>();
        SetSinglePowerupMaterial(baseMaterial);
        powerupImpl = GetComponent<PowerupImplementations>();
        //currentMaterial.SetFloat("_TintValue", currentFadeHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (powerupActive)
        {
            CheckPowerupsActivation();
        }
    }

    #endregion

    #region Class Specific Methods

    void CheckPowerupsActivation()
    {
        if (healthActive)
        {
            HealthPowerupEffect();
        }
        if (staminaActive)
        {
            StaminaPowerupEffect();
        }
        if (shieldActive)
        {
            ShieldPowerupEffect();
        }
    }

    

    void SetSinglePowerupMaterial(Material newMat)
    {
        currentMaterialBody = newMat;
        currentMaterialHead = newMat;
        this.bodyRenderer.material = newMat;
        this.headRenderer.material = newMat;
    }

    void SetSeparatePowerupMaterials(Material bodyMat, Material headMat)
    {
        currentMaterialBody = bodyMat;
        currentMaterialHead = headMat;
        this.bodyRenderer.material = bodyMat;
        this.headRenderer.material = headMat;

    }

    void SetSingleMaterialValue(string fieldToSet, float newValue)
    {
        currentMaterialBody.SetFloat(fieldToSet, newValue);
        currentMaterialHead.SetFloat(fieldToSet, newValue);
    }

    void SetMultipleMaterialValue(float newValue)
    {

    }

    public void SetPowerupActive(bool newBool)
    {
        this.powerupActive = newBool;
    }

    public bool IsPowerUpActive()
    {
        return this.powerupActive;
    }

    public bool IsHealthPowerupEffectActive()
    {
        return this.shieldActive;
    }

    public void SetHealthActive(bool newHealthActive)
    {
        this.healthActive = newHealthActive;
    }

    public bool IsStaminaPowerupEffectActive()
    {
        return this.staminaActive;
    }

    public void SetStaminaActive(bool newStaminaActive)
    {
        this.staminaActive = newStaminaActive;
    }

    public float GetStaminaPowerupTimeInEffect()
    {
        return this.staminaPowerupTimeInEffect;
    }

    public bool IsShieldPowerupEffectActive()
    {
        return this.shieldActive;
    }

    public void SetShieldActive(bool newShieldActive)
    {
        this.shieldActive = newShieldActive;
    }
    #endregion


    #region Health Powerup Methods
    public bool GetHealthActivate()
    {
        return this.healthActivate;
    }
    public void SetHealthActivate(bool newHealthActivate)
    {
        this.healthActivate = newHealthActivate;
    }
    public void SetHealthPowerupMaterial()
    {
        SetSinglePowerupMaterial(healthPowerupMaterial);
    }

    void HealthPowerupEffect()
    {
        if (healthActivate)
        {
            if (currentlyIncreasingHealth && currentFadeHealth < maxHealthPowerupAlpha) 
            {
                currentFadeHealth += (Time.deltaTime * speedHealthPowerup);
            }
            else if (currentlyIncreasingHealth && currentFadeHealth >= maxHealthPowerupAlpha)
            {
                currentlyIncreasingHealth = false;
            }
            else if (!currentlyIncreasingHealth && currentFadeHealth > minHealthPowerupAlpha)
            {
                currentFadeHealth -= (Time.deltaTime * speedHealthPowerdown);
            }
            else if (!currentlyIncreasingHealth && currentFadeHealth <= minHealthPowerupAlpha)
            {
                SetPowerupActive(false);
                SetHealthActive(false);
                healthActivate = false;
                currentlyIncreasingHealth = true;
                currentFadeHealth = minHealthPowerupAlpha;
            }
            SetSingleMaterialValue("_TintValue", currentFadeHealth);
        }
    }

    #endregion


    #region Stamina Powerup Methods
    void StaminaPowerupEffect()
    {
        if (staminaActivate)
        {
            if (currentlyFadingStamina && currentFadeStamina > lowFadeStamina)
            {
                currentFadeStamina -= (Time.deltaTime * staminaEffectSpeed);
            }
            else if (currentlyFadingStamina && currentFadeStamina <= lowFadeStamina)
            {
                currentlyFadingStamina = false;
            }
            else if (!currentlyFadingStamina && currentFadeStamina < highFadeStamina)
            {
                currentFadeStamina += (Time.deltaTime * staminaEffectSpeed);
            }
            else if (!currentlyFadingStamina && currentFadeStamina >= highFadeStamina)
            {
                currentlyFadingStamina = true;
            }
            SetSingleMaterialValue("_Fade", currentFadeStamina);
        }
        else if (currentFadeStamina < noFadeStamina)
        {
            currentFadeStamina += (Time.deltaTime * staminaEffectSpeed);
            SetSingleMaterialValue("_Fade", currentFadeStamina);
        }
        else
        {
            SetPowerupActive(false);
            SetStaminaActive(false);
            SetSinglePowerupMaterial(baseMaterial);
        }
    }
    public bool GetStaminaActivate()
    {
        return this.healthActivate;
    }

    public void SetStaminaActivate(bool newStaminaActivate)
    {
        this.staminaActivate = newStaminaActivate;
    }

    public float GetStaminaPowerupTime()
    {
        return this.staminaPowerupTimeInEffect;
    }
    public void SetStaminaPowerupMaterial()
    {
        SetSinglePowerupMaterial(staminaPowerupMaterial);
    }



    #endregion

    #region Shield Powerup Methods

    public bool GetShieldActivate()
    {
        return this.shieldActivate;
    }

    public void SetShieldActivate(bool newShieldActivate)
    {
        this.shieldActivate = newShieldActivate;
    }


    public float GetShieldPowerupTime()
    {
        return this.shieldPowerupTimeInEffect;
    }
    public void SetShieldPowerupMaterial()
    {
        SetSeparatePowerupMaterials(bodyShieldPowerupMaterial, headShieldPowerupMaterial);
    }

    
    void ShieldPowerupEffect()
    {
        if (shieldActivate)
        {
            if (!currentlyFadingShield && currentFadeShield < maxShieldValue)
            {
                currentFadeShield += (Time.deltaTime * shieldEffectSpeed);
            }
            else if (!currentlyFadingShield && currentFadeShield >= maxShieldValue)
            {
                currentlyFadingShield = true;
            }
            else if (currentlyFadingShield && currentFadeShield > minShieldValue)
            {
                currentFadeShield -= (Time.deltaTime * shieldEffectSpeed);
            }
            else if (currentlyFadingShield && currentFadeShield <= minShieldValue)
            {
                currentlyFadingShield = false;
            }
            SetSingleMaterialValue("_OutlineThickness", currentFadeShield);
        }
        else if (currentFadeShield > noFadeShieldValue)
        {
            currentFadeShield -= (Time.deltaTime * shieldEffectSpeed);
            SetSingleMaterialValue("_OutlineThickness", currentFadeShield);
        }
        else
        {
            print("End of shield powerup effect - setting back to boring shark material and deactivating powerup");
            SetPowerupActive(false);
            SetShieldActive(false);
            powerupImpl.shieldPowerupEffectOff();
            SetSinglePowerupMaterial(baseMaterial);
        }
    }
    #endregion

}
