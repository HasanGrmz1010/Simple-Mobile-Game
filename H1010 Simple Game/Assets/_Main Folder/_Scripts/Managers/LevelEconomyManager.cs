using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEconomyManager : MonoBehaviour
{
    #region Singleton
    public static LevelEconomyManager instance;
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

    private void Start()
    {
        levelStar = 0;
        life = 3;
    }

    public int levelStar;
    public int life;
}
