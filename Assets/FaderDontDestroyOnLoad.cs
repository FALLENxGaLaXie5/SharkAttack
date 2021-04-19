using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderDontDestroyOnLoad : MonoBehaviour
{
    #region Static Variables

    public static FaderDontDestroyOnLoad instance = null;

    #endregion

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
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
