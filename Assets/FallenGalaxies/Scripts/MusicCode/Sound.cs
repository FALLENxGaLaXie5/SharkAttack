using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 1f)]
    public float fadeInVolume;
    [Range(0.1f, 3f)]
    public float startingPitch;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool playOnAwake;
    public bool loop;

    [HideInInspector]
    public AudioSource source;

    #region Class Functions
    public IEnumerator FadeOut(float speed)
    {
        float audioVolume = volume;
        while (volume >= 0f)
        {
            audioVolume -= speed;
            this.volume = audioVolume;
            this.source.volume = this.volume;
            yield return new WaitForSeconds(0.1f);
        }
        this.source.Stop();
    }

    public IEnumerator FadeIn(float speed)
    {
        float audioVolume = volume;
        while(this.volume < this.fadeInVolume)
        {
            audioVolume += speed;
            this.volume = audioVolume;
            this.source.volume = this.volume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetPitch(float newPitch)
    {
        this.pitch = newPitch;
        source.pitch = this.pitch;
    }

    public float GetPitch()
    {
        return this.pitch;
    }

    public float GetStartingPitch()
    {
        return this.startingPitch;
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }

    public void ResetVolume()
    {
        this.volume = 0f;
        this.source.volume = this.volume;
    }
    #endregion
}
