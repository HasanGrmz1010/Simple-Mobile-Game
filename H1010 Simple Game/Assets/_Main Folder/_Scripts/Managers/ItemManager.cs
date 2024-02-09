using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    #region Singleton
    public static ItemManager instance;
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

    // Script Assignments
    [SerializeField] SO_Utility util;

    [SerializeField] float minX, maxX, minZ, maxZ;
    [Header("-------------------------------------------")]
    [SerializeField] int itemAmountValue;
    public List<string> CurrentObjectTags = new List<string>();

    [SerializeField] Transform ITEMS;

    private void Start()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        if (level >= 10)
        {

        }
        else if (level >= 30)
        {

        }
        else if (level >= 60)
        {

        }
        else
        {
            GenerateItems(util.gameItems.Count);
        }
    }

    void GenerateItems(int amount)
    {
        if (amount <= util.gameItems.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject itemToProduce = util.gameItems[i];
                string itemTag = itemToProduce.GetComponent<GameItem>().item_tag;
                for (int x = 0; x < (itemAmountValue / 3); x++)
                {
                    CurrentObjectTags.Add(itemTag);
                }
                for (int j = 0; j < itemAmountValue; j++)
                {
                    GameObject item = Instantiate(itemToProduce, CreateSpawnPoint(), Quaternion.identity, ITEMS);
                }
            }
        }
        
    }

    Vector3 CreateSpawnPoint()
    {
        Vector3 point = new Vector3(Random.Range(minX, maxX), 8f, Random.Range(minZ, maxZ));
        return point;
    }
}
