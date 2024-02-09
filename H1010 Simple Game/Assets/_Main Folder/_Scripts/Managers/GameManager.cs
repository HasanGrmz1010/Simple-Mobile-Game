using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null && this != instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    public enum GameState
    {
        playing,
        timeUp_GameOver,
        noLife_GameOver,
        orderComplete_LevelPassed
    }
    public GameState state = new GameState();


}
