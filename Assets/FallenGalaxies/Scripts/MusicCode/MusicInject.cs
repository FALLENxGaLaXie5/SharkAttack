using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInject : MonoBehaviour
{
    [SerializeField] string injectTrackName;

    void Awake()
    {
        if (injectTrackName != null)
        {
            FindObjectOfType<MusicControl>().FadeIn(injectTrackName);
        }
        else
        {
            Debug.LogWarning("Could not find track! Are you starting from the main menu?");
        }
    }
}
