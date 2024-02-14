using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuGUIManager : MonoBehaviour
{
    #region Singleton
    public static MainMenuGUIManager instance;
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

    [SerializeField] SO_GameData gameData;
    [SerializeField] Ease menuEase;

    [Header("______________ IMAGES ______________")]
    [SerializeField] Sprite level_chest_img;
    [SerializeField] Sprite star_chest_img;
    [SerializeField] Image chest_arrow;

    [Header("______________ TEXTS ______________")]
    [SerializeField] TextMeshProUGUI coinValueText;
    [SerializeField] TextMeshProUGUI starValueText;
    [SerializeField] TextMeshProUGUI openChestText;

    [Header("______________ PANELS ______________")]
    [SerializeField] Image OpenChestScreen_img;
    [SerializeField] RectTransform OpenChestPanel;
    [SerializeField] Image settingsBackground;
    [SerializeField] RectTransform settingsMenu;
    [SerializeField] RectTransform allPages;
    [SerializeField] Image curtain;

    [Header("______________ BUTTONS ______________")]
    [SerializeField] Button revealChestButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button settingCloseButton;
    [SerializeField] Button settingsOpenButton;
    [SerializeField] List<RectTransform> MM_buttons = new List<RectTransform>();

    float frontPageX = 0f, storePageX = 800f, scoreboardPageX = -800f;
    private void OnDisable()
    {
        continueButton.onClick.RemoveAllListeners();
        settingCloseButton.onClick.RemoveAllListeners();
        settingsOpenButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        int levelChestProgress = gameData.LEVEL % 5;

        // Button Subscribtions
        continueButton.onClick.AddListener(ContinueGame_Button);
        settingCloseButton.onClick.AddListener(Settings_CloseButton);
        settingsOpenButton.onClick.AddListener(Settings_OpenButton);

        // Continue Button Loop Tween
        continueButton.transform.DOScale(1.1f, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            continueButton.transform.DOScale(1f, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        });

        coinValueText.text = EconomyManager.instance.TotalCoin.ToString();
        starValueText.text = EconomyManager.instance.TotalStar.ToString();
    }

    #region MM Page Functions
    // MM PAGE FUNCTIONS -------------------------------------------------------
    public void Button_StorePage()
    {
        allPages.DOLocalMoveX(storePageX, .5f).SetEase(menuEase);
        MM_buttons[0].DOScale(1.2f, .2f).SetEase(menuEase);

        MM_buttons[1].DOScale(1f, .2f).SetEase(menuEase);
        MM_buttons[2].DOScale(1f, .2f).SetEase(menuEase);
    }

    public void Button_FrontPage()
    {
        allPages.DOLocalMoveX(frontPageX, .5f).SetEase(menuEase);
        MM_buttons[1].DOScale(1.2f, .2f).SetEase(menuEase);

        MM_buttons[0].DOScale(1f, .2f).SetEase(menuEase);
        MM_buttons[2].DOScale(1f, .2f).SetEase(menuEase);
    }

    public void Button_ScorePage()
    {
        allPages.DOLocalMoveX(scoreboardPageX, .5f).SetEase(menuEase);
        MM_buttons[2].DOScale(1.2f, .2f).SetEase(menuEase);

        MM_buttons[0].DOScale(1f, .2f).SetEase(menuEase);
        MM_buttons[1].DOScale(1f, .2f).SetEase(menuEase);
    }
    #endregion


    // SETTINGS FUNCTIONS -------------------------------------------------------
    void Settings_OpenButton()
    {
        settingsMenu.gameObject.SetActive(true);
        settingsBackground.gameObject.SetActive(true);
        settingsBackground.DOColor(new Color(0, 0, 0, .85f), .25f);
        settingsMenu.DOLocalMoveY(0f, .4f).SetEase(menuEase);
    }

    void Settings_CloseButton() {
        settingsBackground.DOColor(new Color(0, 0, 0, 0), .25f);
        settingsMenu.DOLocalMoveY(-1400f, .4f).SetEase(menuEase).OnComplete(() =>
        {
            settingsBackground.gameObject.SetActive(false);
            settingsMenu.gameObject.SetActive(false);
        });
    }

    // 11111 FUNCTIONS --------------------------------------------------------
    void ContinueGame_Button()
    {
        curtain.gameObject.SetActive(true);
        curtain.DOColor(new Color(0f, 0f, 0f, 1f), .35f).OnComplete(() =>
        {
            SceneManager.LoadScene(gameData.LEVEL);
        });
    }

    // Misc FUNCTIONS -------------------------------------------------------------------
    public void ChestRevealMenu_Button(string _chestType)
    {
        switch (_chestType)
        {
            case "level":
                revealChestButton.image.sprite = level_chest_img;
                break;
            case "star":
                revealChestButton.image.sprite = star_chest_img;
                break;
            default:
                break;
        }

        revealChestButton.transform.DORotate(new Vector3(0f, 0f, 15f), 1f, RotateMode.FastBeyond360).
        SetEase(Ease.InOutSine).
        OnComplete(() =>
        {
            revealChestButton.transform.DOScale(1.1f, .5f).
            SetEase(Ease.InOutCirc).
            OnComplete(() =>
            {
                revealChestButton.transform.DOScale(1f, .5f).
                SetEase(Ease.InOutCirc).
                SetLoops(-1, LoopType.Yoyo);
            });

            revealChestButton.transform.DORotate(new Vector3(0f, 0f, -15f), 1f, RotateMode.FastBeyond360).
            SetEase(Ease.InOutSine).
            SetLoops(-1, LoopType.Yoyo);
        });

        chest_arrow.rectTransform.DOLocalMoveY(chest_arrow.rectTransform.localPosition.y - 85f, .5f).
            SetEase(Ease.InOutCubic).
            OnComplete(() =>
            {
                chest_arrow.rectTransform.DOLocalMoveY(chest_arrow.rectTransform.localPosition.y + 85f, .5f).
                SetEase(Ease.InOutCubic).
                SetLoops(-1, LoopType.Yoyo);
            });

        OpenChestScreen_img.gameObject.SetActive(true);
        OpenChestScreen_img.DOColor(new Color(0f, 0f, 0f, .85f), .3f);
        OpenChestPanel.DOScale(1f, .4f).SetEase(Ease.OutBack);
    }
    public void OpenChest_Button()
    {
        if (revealChestButton.image.sprite == level_chest_img)
        {
            Debug.Log("LEVEL CHEST");
        }
        else if (revealChestButton.image.sprite == star_chest_img)
        {
            Debug.Log("STAR CHEST");
        }
    }
}
