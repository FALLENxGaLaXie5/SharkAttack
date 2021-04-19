using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingScript : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] int damage = 1;
    [SerializeField] float eatTime = 3f;
    [SerializeField] ParticleSystem bloodParticles;

    #endregion

    #region Components
    Animator animator;
    AudioSource audioSource;
    PlayerPowerupEffects powerupsScript;
    PowerupImplementations powerupImplementations;
    #endregion

    #region Class Variables
    float timeLeftToEat = 0f;
    bool isEating = false;
    bool triggeredEating = false;
    #endregion

    #region Unity Event Functions
    // Start is called before the first frame update
    void Start()
    {
        powerupsScript = transform.parent.GetComponent<PlayerPowerupEffects>();
        powerupImplementations = transform.parent.GetComponent<PowerupImplementations>();
        animator = transform.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timeLeftToEat > 0)
        {
            timeLeftToEat -= Time.deltaTime;
            if (!triggeredEating)
            {
                bloodParticles.Play();
                triggeredEating = true;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Eatable")
        {
            string fishTag = collision.gameObject.transform.parent.gameObject.tag;
            collision.gameObject.GetComponent<IEatable>().Eaten(damage);
            animator.SetTrigger("Close");
            audioSource.Play();
            ResetEatTime();
            if (!powerupsScript.IsPowerUpActive() && !fishTag.Equals("Fish"))
            {                
                TriggerPowerup(fishTag);
            }


            if (collision.gameObject.transform.parent.gameObject.tag == "FishBomb")
            {

            }
        }
    }

    #endregion

    #region Class Functions
    void TriggerPowerup(string fishTag)
    {
        if (fishTag == "FishHealth")
        {
            powerupImplementations.healthPowerupEffect();
            powerupsScript.SetPowerupActive(true);
            powerupsScript.SetHealthPowerupMaterial();
            powerupsScript.SetHealthActivate(true);
            powerupsScript.SetHealthActive(true);
        }
        else if (fishTag == "FishStamina")
        {
            StartCoroutine(powerupImplementations.staminaPowerupEffect());
            powerupsScript.SetPowerupActive(true);
            powerupsScript.SetStaminaPowerupMaterial();
            powerupsScript.SetStaminaActivate(true);
            powerupsScript.SetStaminaActive(true);
            StartCoroutine(StaminaPowerupTimer());
        }
        else if (fishTag == "FishShield")
        {
            powerupImplementations.shieldPowerupEffectOn();
            powerupsScript.SetPowerupActive(true);
            print("I hit a shield fishy!");
            powerupsScript.SetShieldPowerupMaterial();
            powerupsScript.SetShieldActivate(true);
            powerupsScript.SetShieldActive(true);
            StartCoroutine(ShieldPowerupTimer());
        }
    }

    void ResetEatTime()
    {
        timeLeftToEat = eatTime;
        triggeredEating = false;
    }

    IEnumerator StaminaPowerupTimer()
    {
        yield return new WaitForSeconds(powerupsScript.GetStaminaPowerupTime());
        powerupsScript.SetStaminaActivate(false);
    }

    IEnumerator ShieldPowerupTimer()
    {
        yield return new WaitForSeconds(powerupsScript.GetShieldPowerupTime());
        powerupsScript.SetShieldActivate(false);
    }

    #endregion
}
