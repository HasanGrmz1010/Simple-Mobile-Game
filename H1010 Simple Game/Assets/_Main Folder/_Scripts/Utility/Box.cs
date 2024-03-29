using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Script Assignments
    [SerializeField] SO_Utility util;

    List<GameItem> boxContent = new List<GameItem>();
    [SerializeField] string order_tag;
    public float box_speed;
    float temp_speed_val;

    private void OnEnable()
    {
        LevelEventManager.onTimesUp += GameOver_Behaviour;
        LevelEventManager.onNoLifeRemains += GameOver_Behaviour;
        LevelEventManager.onLevelPassed += GameOver_Behaviour;
        
        LevelEventManager.onPausedGame += PausedGame_Behaviour;
        LevelEventManager.onResumedGame += ResumeGame_Behaviour;
    }

    private void OnDisable()
    {
        LevelEventManager.onTimesUp -= GameOver_Behaviour;
        LevelEventManager.onNoLifeRemains -= GameOver_Behaviour;
        LevelEventManager.onLevelPassed -= GameOver_Behaviour;
        
        LevelEventManager.onPausedGame -= PausedGame_Behaviour;
        LevelEventManager.onResumedGame -= ResumeGame_Behaviour;
    }

    private void Start()
    {
        order_tag = ItemManager.instance.CurrentObjectTags[Random.Range(0, ItemManager.instance.CurrentObjectTags.Count)];
        AssignItemIcons();
    }

    private void Update()
    {
        transform.position += new Vector3(-box_speed * Time.deltaTime, 0, 0);
    }

    void AssignItemIcons()
    {
        foreach (Sprite icon in util.itemIcons)
        {
            if (icon.name == order_tag)
            {
                foreach (Transform child in transform.GetChild(0))
                {
                    if (child.name == icon.name)
                    {
                        child.gameObject.SetActive(true);
                        return;
                    }
                }
                return;
            }
        }
    }

    void OrderComplete()
    {
        int correctItemCount = 0;
        foreach (GameItem item in boxContent)
        {
            if (item.item_tag == order_tag)
            {
                correctItemCount++;
            }
        }
        if (correctItemCount == 3)
        {
            Debug.Log("BOX SUCCESS");
            LevelEconomyManager.instance.levelStar++;
            box_speed = 0f;
            ItemManager.instance.CurrentObjectTags.Remove(order_tag);
            InGameGUIManager.instance.BoxSuccess_GUI_Handle();
            transform.DORotate(Vector3.up * 180, .5f, RotateMode.FastBeyond360).SetEase(Ease.InCubic);
            transform.DOScale(.01f, .5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }
        else
        {
            Debug.Log("BOX FAIL");
            if (LevelEconomyManager.instance.life > 0)
            {
                LevelEconomyManager.instance.life--;
                InGameGUIManager.instance.BoxFail_GUI_Handle();
                if (LevelEconomyManager.instance.life == 0)
                {
                    LevelEventManager.onNoLifeRemains();
                }
            }
            transform.DORotate(Vector3.up * 360, .5f, RotateMode.FastBeyond360).SetEase(Ease.InCubic);
            transform.DOScale(.01f, .5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(this.gameObject);
                //this.gameObject.SetActive(false);
            });
        }
    }

    public void AddItemToBox(GameItem item)
    {
        BoxManager bm = BoxManager.instance;
        if (item != null && boxContent.Count < 3)
        {
            boxContent.Add(item);
            if (boxContent.Count == 3)
            {
                bm.allBoxes.Remove(this.gameObject);
                bm.currentBox = null;
                if (bm.allBoxes.Count != 0)
                {
                    bm.currentBox = bm.allBoxes[0].transform;
                }
                
                OrderComplete();
            }
        }
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "box_dump")
        {
            transform.DORotate(Vector3.up * 180, .5f, RotateMode.FastBeyond360).SetEase(Ease.InCubic);
            transform.DOScale(.01f, .5f).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                Destroy(this.gameObject);
                //this.gameObject.SetActive(false);
            });
        }
    }

    void GameOver_Behaviour()
    {
        box_speed = 0f;
        transform.DORotate(Vector3.up * 360, .5f, RotateMode.FastBeyond360).SetEase(Ease.InCubic).SetDelay(1f);
        transform.DOScale(.01f, .5f).SetEase(Ease.InBack).SetDelay(1f).OnComplete(() =>
        {
            Destroy(this.gameObject);
            //this.gameObject.SetActive(false);
        });
    }

    void PausedGame_Behaviour()
    {
        temp_speed_val = box_speed;
        box_speed = 0f;
    }

    void ResumeGame_Behaviour()
    {
        box_speed = temp_speed_val;
    }
}
