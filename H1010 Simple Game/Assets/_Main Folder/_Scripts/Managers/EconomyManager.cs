using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    #region Singleton
    public static EconomyManager instance;
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

    static private int TotalCoin;
    static private int TotalGem;
}
