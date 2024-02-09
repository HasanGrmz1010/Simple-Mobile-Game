using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameGUIManager : MonoBehaviour
{
    #region Singleton
    public static InGameGUIManager instance;
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

    [SerializeField] Canvas inGameCanvas;
    [SerializeField] GameObject star_img;

    [SerializeField] SO_GameData gameData;
    [SerializeField] RectTransform starSpawnPoint;

    [Header("___________ PANELS ___________")]
    [SerializeField] Image GameOverScreen_img;
    [SerializeField] RectTransform GameOverPanel;
    [SerializeField] RectTransform starIndicator;

    [Header("___________ TEXTS ___________")]
    [SerializeField] TextMeshProUGUI starAmountText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI gameOverReasonText;

    [Header("___________ BUTTONS ___________")]
    [SerializeField] Button pauseButton;

    private void OnEnable()
    {
        // Event Subscribtions
        LevelEventManager.onTimesUp += NoTimeLeft_GameOverScreen;
    }

    private void OnDisable()
    {
        LevelEventManager.onTimesUp -= NoTimeLeft_GameOverScreen;
    }

    private void Start()
    {
        levelText.text = "LEVEL " + SceneManager.GetActiveScene().buildIndex.ToString();
    }

    public void BoxSuccess_GUI_Handle()
    {
        GameObject star = Instantiate(star_img, starSpawnPoint.position, Quaternion.Euler(45f,0f,0f), inGameCanvas.transform);
        star.transform.DOScale(1f, .5f).OnComplete(() =>
        {
            star.transform.DOScale(.75f, .5f);
        });
        star.transform.DOMove(starIndicator.GetChild(0).GetChild(1).position, 1.25f).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            Destroy(star.gameObject);
            starIndicator.DOPunchScale(Vector3.one / 5f, .35f, 1, .5f);
            LevelEconomyManager.instance.levelStar++;
            starAmountText.text = LevelEconomyManager.instance.levelStar.ToString();
        });
        
    }

    public void PauseMenuButton()
    {

    }

    // GAME STATE FUNCTIONS ---------------------------------------------------
    void NoTimeLeft_GameOverScreen()
    {
        GameOverScreen_img.gameObject.SetActive(true);
        GameOverScreen_img.DOColor(new Color(0f, 0f, 0f, .85f), 1f).SetEase(Ease.OutCubic);
        gameOverReasonText.rectTransform.DOLocalMoveX(0f, .5f).SetEase(Ease.OutCirc);
        GameOverPanel.DOLocalMoveX(0f, .5f).SetEase(Ease.OutCirc);
    }

    void NoLife_GameOverScreen()
    {

    }
}
