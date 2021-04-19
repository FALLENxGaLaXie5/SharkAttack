using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        TransitionScenePersistent transitionsceneObject = FindObjectOfType<TransitionScenePersistent>();
        if (transitionsceneObject != null)
        {
            transitionsceneObject.BeginFadeIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
