using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupImplementations : MonoBehaviour
{
    [SerializeField] int numHealthRegenerate = 1;
    EatableShark eatableShark;
    PlayerPowerupEffects powerupVisualEffects;
    StaminaBar staminaBar;
    #region Unity Event Methods
    // Start is called before the first frame update
    void Start()
    {
        eatableShark = GetComponentInChildren<EatableShark>();
        powerupVisualEffects = GetComponent<PlayerPowerupEffects>();
        staminaBar = GetComponentInChildren<StaminaBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Class Methods
    public void healthPowerupEffect()
    {
        eatableShark.RegenerateHealth(numHealthRegenerate);
    }

    public IEnumerator staminaPowerupEffect()
    {
        staminaBar.SetPowered(true);
        yield return new WaitForSeconds(powerupVisualEffects.GetStaminaPowerupTimeInEffect());
        staminaBar.SetPowered(false);
    }

    public void shieldPowerupEffectOn()
    {
        eatableShark.SetShielded(true);
    }

    public void shieldPowerupEffectOff()
    {
        eatableShark.SetShielded(false);
    }
    #endregion
}
