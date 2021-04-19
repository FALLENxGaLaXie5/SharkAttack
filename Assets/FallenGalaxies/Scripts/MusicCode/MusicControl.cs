using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class MusicControl : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] float musicFadeSpeed = 0.05f;
    [SerializeField] float musicFadeInSpeed = 0.05f;
    public Sound[] sounds;
    public Coroutine currentMusicCoroutine;
    AudioSource audioSource;

    #endregion


    #region Class Variables
    //Used to control functionality for when game cycles back to main menu
    bool firstTimePlay = true;

    public Tuple<Sound, int> currentSoundtrack;

    #endregion


    #region Static Variables

    public static MusicControl instance = null;
    
    #endregion

    #region Unity Event Functions

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.startingPitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }    
    }

    void Start()
    {
        PlaySoundtrack("Track1");
    }

    #endregion

    #region Class Functions
    public void PlaySoundtrack(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlaySoundEffect(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void FadeOut(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        currentMusicCoroutine = StartCoroutine(s.FadeOut(musicFadeSpeed));
    }

    public void FadeIn(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.ResetVolume();
        s.source.Play();

        currentMusicCoroutine = StartCoroutine(s.FadeIn(musicFadeInSpeed));
    }

    void StopCurrentCoroutine()
    {
        StopCoroutine(currentMusicCoroutine);
    }
    public IEnumerator FadeOutOldFadeInNew(string oldTrack, string newTrack)
    {
        Sound oldOne = Array.Find(sounds, sound => sound.name == oldTrack);
        Sound newOne = Array.Find(sounds, sound => sound.name == newTrack);

        StartCoroutine(oldOne.FadeOut(musicFadeSpeed));
        yield return new WaitForSeconds(5.0f);
        print("IM DONE WITH FIRST MUSIC");
        newOne.source.Play();
        StartCoroutine(newOne.FadeIn(musicFadeInSpeed));
    }

    public IEnumerator LerpToNewPitch(string name, float oldPitch, float newPitch, float speed)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        while (oldPitch < newPitch)
        {
            oldPitch += speed;
            s.SetPitch(oldPitch);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public float GetCurrentPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.GetPitch();
    }

    public float GetInitialPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.GetStartingPitch();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.IsPlaying();
    }

    public bool IsFirstTimePlay()
    {
        return this.firstTimePlay;
    }

    public void SetFirstTimePlay(bool newFirstTimePlay)
    {
        this.firstTimePlay = newFirstTimePlay;
    }
    #endregion
}
