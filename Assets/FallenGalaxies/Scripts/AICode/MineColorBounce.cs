using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class MineColorBounce : MonoBehaviour
{
    enum AlphaTransition
    {
        AlphaToOpaque,
        OpaqueToAlpha
    }

    [SerializeField] float interpolateDuration = 4f;
    [SerializeField] GameObject lightObject;
    [SerializeField] float maxLightIntensity = 2f;
    [SerializeField] Color fullColor = new Color(255, 255, 255, 100);

    float duration;
    float mLerp;
    float mLLerp;
    AlphaTransition currentTransition;
    Light light2D;

    Color transparentColor = new Color(255,0,0,0);
    SpriteRenderer renderer;

    UnityEngine.Experimental.Rendering.Universal.Light2D light2DComponent;
    

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        currentTransition = AlphaTransition.OpaqueToAlpha;
        duration = interpolateDuration;
        light2DComponent = lightObject.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PingPongColor();
        PingPongLightIntensity();
    }
    
    void PingPongColor()
    {
        if (currentTransition == AlphaTransition.OpaqueToAlpha)
        {
            duration -= Time.deltaTime;
            mLerp = (interpolateDuration - duration) / interpolateDuration;
            renderer.color = Color.Lerp(fullColor, transparentColor, mLerp);
            if (mLerp >= 1)
            {
                currentTransition = AlphaTransition.AlphaToOpaque;
                duration = interpolateDuration;
            }
        }
        else
        {
            duration -= Time.deltaTime;
            mLerp = (interpolateDuration - duration) / interpolateDuration;
            renderer.color = Color.Lerp(transparentColor, fullColor, mLerp);
            if (mLerp >= 1)
            {
                currentTransition = AlphaTransition.OpaqueToAlpha;
                duration = interpolateDuration;
            }
        }
    }

    void PingPongLightIntensity()
    {
        if (currentTransition == AlphaTransition.OpaqueToAlpha)
        {
            mLLerp = (interpolateDuration - duration) / interpolateDuration;
            light2DComponent.intensity = Mathf.Lerp(maxLightIntensity, 0f, mLLerp);
        }
        else
        {
            mLLerp = (interpolateDuration - duration) / interpolateDuration;
            light2DComponent.intensity = Mathf.Lerp(0f, maxLightIntensity, mLLerp);
        }
    }
}
