using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    [Header("______________ TEXTS ______________")]
    [SerializeField] TextMeshProUGUI coinValueText;
    

    [Header("______________ PANELS ______________")]
    [SerializeField] Image settingsBackground;
    [SerializeField] RectTransform settingsMenu;
    [SerializeField] RectTransform allPages;

    [Header("______________ BUTTONS ______________")]
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
        // Button Subscribtions
        continueButton.onClick.AddListener(ContinueGame_Button);
        settingCloseButton.onClick.AddListener(Settings_CloseButton);
        settingsOpenButton.onClick.AddListener(Settings_OpenButton);

        // Loop Tween Set
        continueButton.transform.DOScale(1.1f, .75f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            continueButton.transform.DOScale(1f, .75f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        });

        coinValueText.text = gameData.COIN.ToString();
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
        SceneManager.LoadScene(gameData.LEVEL);
    }
}
