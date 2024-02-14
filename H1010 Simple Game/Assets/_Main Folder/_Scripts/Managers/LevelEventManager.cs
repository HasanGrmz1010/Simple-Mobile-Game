using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager : MonoBehaviour
{
    #region Singleton
    public static LevelEventManager instance;
    private void Awake()
    {
        if (instance != null && this != instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public bool ableToSelect = false;

    public static Action onTimesUp;
    public static Action onNoLifeRemains;
    public static Action onPausedGame;
    public static Action onResumedGame;
    public static Action onLevelPassed;
}
