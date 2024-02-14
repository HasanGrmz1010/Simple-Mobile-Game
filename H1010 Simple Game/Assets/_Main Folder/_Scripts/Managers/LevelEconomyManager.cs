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

        starGoal = Random.Range(7, 10);
    }

    public int starGoal;
    public int levelStar;
    public int life;

    public bool hasReachedStarGoal()
    {
        return (levelStar == starGoal);
    }
}
