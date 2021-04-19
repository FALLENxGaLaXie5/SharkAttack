using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


/**
 * This manager will not be present in between scenes - will be used just to control timelines in level 1
 */
public class TimelineManager : MonoBehaviour
{
    public PlayableDirector[] timelines;
    public static TimelineManager instance = null;

    [SerializeField] float sunTimelineIncrement = 1f;
    float lastTimelineTimeMarker;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        lastTimelineTimeMarker = sunTimelineIncrement;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSunEffectTimelineForPause();
    }

    void CheckSunEffectTimelineForPause()
    {
        if (timelines[0].time >= lastTimelineTimeMarker)
        {
            lastTimelineTimeMarker += sunTimelineIncrement;
            timelines[0].Pause();
        }
    }

    public void PlayTimeline(int i)
    {
        timelines[i].Play();
    }

    public void StopTimeline(int i)
    {
        timelines[i].Stop();
    }

    public void PauseTimeline(int i)
    {
        timelines[i].Pause();
    }
}
