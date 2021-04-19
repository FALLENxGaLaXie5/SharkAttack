using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatLightBrain : MonoBehaviour
{
    BoatBrain boatBrain;
    [SerializeField] float chaseSpeed = 10f;
    [SerializeField] float minChaseSmokeSpawnSpeed = 0.5f;
    [SerializeField] float maxChaseSmokeSpawnSpeed = 1.0f;
    ParticleSystem smoke;
    ParticleSystem.EmissionModule emissionModule;

    ParticleSystem.MinMaxCurve beginEmissionRateOverTimeCurve;
    
    // Start is called before the first frame update
    void Start()
    {
        smoke = transform.parent.parent.GetComponentInChildren<ParticleSystem>();
        emissionModule = smoke.emission;
        beginEmissionRateOverTimeCurve = emissionModule.rateOverTime;
        boatBrain = transform.parent.parent.GetComponentInChildren<BoatBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minChaseSmokeSpawnSpeed, maxChaseSmokeSpawnSpeed);
            float speedDifference = boatBrain.GetCurrentSpeed() - boatBrain.GetMinSpeed();
            float percentSpeedLerped = speedDifference / boatBrain.GetTotalSpeedDifference();
            boatBrain.SetSighted(true);
            boatBrain.SetStartTime(percentSpeedLerped * boatBrain.GetTotalSpeedChangeTime());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            emissionModule.rateOverTime = beginEmissionRateOverTimeCurve;
            float speedDifference = boatBrain.GetMaxSpeed() - boatBrain.GetCurrentSpeed();
            float percentSpeedLerped = speedDifference / boatBrain.GetTotalSpeedDifference();
            boatBrain.SetSighted(false);
            boatBrain.SetStartTime(percentSpeedLerped * boatBrain.GetTotalSpeedChangeTime());
        }
    }
}
