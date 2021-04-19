using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInjectMenu : MonoBehaviour
{
    void Start()
    {
        if (MusicControl.instance.IsPlaying("Track2"))    
        {
            MusicControl.instance.FadeOut("Track2");
            MusicControl.instance.FadeIn("Track1");
        }
    }
}
