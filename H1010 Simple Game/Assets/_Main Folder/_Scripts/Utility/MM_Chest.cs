using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MM_Chest : MonoBehaviour
{
    [Header("----------------- TAG -----------------")]
    public string _tag;
    [Header("_______________________________________")]
    [SerializeField] SO_GameData gameData;
    [SerializeField] List<Image> progressBars = new List<Image>();
    [SerializeField] Image starChestFill_img;
    [SerializeField] TextMeshProUGUI progressText;
    private void Start()
    {
        if(_tag == "level")
        {
            int progress = gameData.LEVEL % 5;
            progressText.text = "Level " + gameData.LEVEL.ToString();
            for (int i = 0; i < progress; i++)
            {
                progressBars[i].gameObject.SetActive(true);
            }
            if (progress == 0)
            {
                this.GetComponent<Button>().interactable = true;
                // GIVE CHEST BEHAVIOUR
            }
        }
        else if (_tag == "star")
        {
            float progress = ((float)EconomyManager.instance.TotalStar / 200f);
            starChestFill_img.fillAmount = progress;
            progressText.text = EconomyManager.instance.TotalStar.ToString() + " / 200";
            if (progress == 1f)
            {
                this.GetComponent<Button>().interactable = true;
            }
        }

        this.GetComponent<Button>().onClick.AddListener(Button_Chest);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        this.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    void Button_Chest()
    {
        switch (_tag)
        {
            case "level":

                break;
            case "star":

                break;
            default:
                break;
        }
    }
}
