using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    #region Inspector/Public Variables

    [SerializeField] Image staminaImage;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float rechargeRate = 0.2f;
    [SerializeField] float rechargeBlinkRate = 0.1f;
    [SerializeField] float holdTimeToRecharge = 2f;
    [SerializeField] Color baseColor;
    [SerializeField] Color blinkedColor;

    [Space(5)]

    [Header("Stamina Powerup Variables")]
    [SerializeField] float powerupStaminaRechargeRate = 1.5f;
    [SerializeField] float powerupStaminaRechargeBlinkRate = 0.3f;
    [SerializeField] float powerupHoldTimeToRecharge = 1f;

    public bool powered = false;
    #endregion

    float currentStamina;
    public bool recharging;
    float perc;
    PlayerControl playerController;
    // Start is called before the first frame update
    void Start()
    {
        recharging = true;
        currentStamina = maxStamina;
        perc = currentStamina / maxStamina;
        playerController = transform.parent.GetComponent<PlayerControl>();
        StartCoroutine(BlinkColorRecharging());
    }

    // Update is called once per frame
    void Update()
    {
        RechargeStamina();
    }

    void RechargeStamina()
    {
        if (perc < 1.0)
        {
            if (recharging)
            {
                if (powered)
                {
                    perc += Time.deltaTime * powerupStaminaRechargeRate;
                }
                else
                {
                    perc += Time.deltaTime * rechargeRate;
                }
                currentStamina = Mathf.Lerp(0f, maxStamina, perc);
                staminaImage.fillAmount = currentStamina / maxStamina;
            }
            else
            {
                currentStamina = Mathf.Lerp(0f, maxStamina, perc);
                staminaImage.fillAmount = currentStamina / maxStamina;
            }
        }
        else
        {
            SetRecharging(false);
        }
    }

    IEnumerator BlinkColorRecharging()
    {
        while (true)
        {
            if (recharging)
            {
                if (staminaImage.color == baseColor)
                {
                    staminaImage.color = blinkedColor;
                }
                else
                {
                    staminaImage.color = baseColor;
                }
                if (powered)
                {
                    yield return new WaitForSeconds(powerupStaminaRechargeBlinkRate);
                }
                else
                {
                    yield return new WaitForSeconds(rechargeBlinkRate);
                }
            }
            else
            {
                staminaImage.color = baseColor;
                yield return null;
            }
        }        
    }

    public IEnumerator StartRecharging()
    {
        SetRecharging(false);
        if (powered)
        {
            yield return new WaitForSeconds(powerupHoldTimeToRecharge);
        }
        else
        {
            yield return new WaitForSeconds(holdTimeToRecharge);
        }
        SetRecharging(true);
    }

    #region Getter/Setter Functions

    public bool IsPowered()
    {
        return this.powered;
    }

    public void SetPowered(bool newPowered)
    {
        this.powered = newPowered;
    }
    public float GetCurrentStamina()
    {
        return this.currentStamina;
    }

    public void SetCurrentStamina(float newStamina)
    {
        this.currentStamina = newStamina;
    }

    public float GetPercentStamina()
    {
        return this.perc;
    }

    public void SetPercentStamina(float newPerc)
    {
        this.perc = newPerc;
    }

    public float GetMaxStamina()
    {
        return this.maxStamina;
    }

    public bool GetRecharging()
    {
        return this.recharging;
    }

    public void SetRecharging(bool newRecharging)
    {
        this.recharging = newRecharging;
    }
    #endregion
}
