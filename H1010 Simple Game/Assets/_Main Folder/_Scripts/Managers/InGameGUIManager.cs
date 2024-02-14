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

    int lifeHeart_index;
    [SerializeField] Canvas inGameCanvas;
    [SerializeField] GameObject star_img;
    [SerializeField] SO_GameData gameData;
    [SerializeField] RectTransform starSpawnPoint;

    [Header("___________ PANELS ___________")]
    
    [SerializeField] Image GameOverScreen_img;
    [SerializeField] Image PauseMenuScreen_img;
    [SerializeField] Image LevelPassedScreen_img;
    [SerializeField] RectTransform GameOverPanel;
    [SerializeField] RectTransform PauseMenuPanel;
    [SerializeField] RectTransform LevelPassedPanel;
    [SerializeField] RectTransform starIndicator;
    [SerializeField] Image curtain;

    [Header("___________ TEXTS ___________")]
    [SerializeField] TextMeshProUGUI starAmountText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI gameOverReasonText;
    [SerializeField] TextMeshProUGUI gameOverStarValueText;
    [SerializeField] TextMeshProUGUI levelPassedText;
    [SerializeField] TextMeshProUGUI levelPassedStarValueText;
    [SerializeField] TextMeshProUGUI levelStarGoalText;
    

    [Header("___________ BUTTONS ___________")]
    [SerializeField] Button pauseButton;
    [SerializeField] Button retryButton;
    [SerializeField] Button nextLevelButton;

    [Header("___________ ICONS ___________")]
    [SerializeField] Image gameOverStarIMG;
    [SerializeField] Image levelPassedStarIMG;
    [SerializeField] Image adIcon;
    [SerializeField] List<Image> hearts = new List<Image>();

    private void OnEnable()
    {
        // Event Subscribtions
        LevelEventManager.onTimesUp += NoTimeLeft_GameOverScreen;
        LevelEventManager.onNoLifeRemains += NoLife_GameOverScreen;
        LevelEventManager.onLevelPassed += LevelPassed_Screen;
    }

    private void OnDisable()
    {
        LevelEventManager.onTimesUp -= NoTimeLeft_GameOverScreen;
        LevelEventManager.onNoLifeRemains -= NoLife_GameOverScreen;
        LevelEventManager.onLevelPassed -= LevelPassed_Screen;
    }

    private void Start()
    {
        lifeHeart_index = 2;
        levelStarGoalText.text = "GOAL " + LevelEconomyManager.instance.starGoal.ToString();
        curtain.gameObject.SetActive(true);
        curtain.DOColor(new Color(0f, 0f, 0f, 0f), .35f).OnComplete(() =>
        {
            curtain.gameObject.SetActive(false);
        });
        levelText.text = "LEVEL " + SceneManager.GetActiveScene().buildIndex.ToString();
    }

    public void BoxFail_GUI_Handle()
    {
        hearts[lifeHeart_index].transform.DOScale(.025f, .4f).
        SetEase(Ease.InBack).
        OnComplete(() =>
        {
            hearts[lifeHeart_index].gameObject.SetActive(false);
            lifeHeart_index--;
        });
        
    }

    public void BoxSuccess_GUI_Handle()
    {
        GameObject star = Instantiate(star_img, starSpawnPoint.position,
            Quaternion.Euler(45f,0f,0f),
            inGameCanvas.transform);

        star.transform.DOScale(1f, .5f).OnComplete(() =>
        {
            star.transform.DOScale(.75f, .5f);
        });

        star.transform.DOMove(starIndicator.GetChild(0).GetChild(1).position, 1f).
            SetEase(Ease.InOutCubic).
            OnComplete(() =>
        {
            Destroy(star.gameObject);
            starIndicator.DOPunchScale(Vector3.one / 5f, .35f, 1, .5f);
            starAmountText.text = LevelEconomyManager.instance.levelStar.ToString();
            
            if (LevelEconomyManager.instance.hasReachedStarGoal())
            {
                LevelEventManager.onLevelPassed();
            }
        });
        
    }

    // BUTTON FUNCTIONS ----------------------------------------------------------------
    public void PauseMenu_Button()
    {
        GameManager.instance.state = GameManager.GameState.paused;
        LevelEventManager.onPausedGame();
        PauseMenuScreen_img.gameObject.SetActive(true);
        PauseMenuScreen_img.DOColor(new Color(.78f, 0f, .4f, .88f), .5f);
        PauseMenuPanel.DOLocalMoveY(0f, .5f).SetEase(Ease.OutBack);
    }

    public void ResumeGame_Button()
    {
        GameManager.instance.state = GameManager.GameState.playing;
        PauseMenuPanel.DOLocalMoveY(-2000f, .5f).SetEase(Ease.InBack);
        PauseMenuScreen_img.DOColor(new Color(0f, 0f, 0f, 0f), .6f).OnComplete(() =>
        {
            PauseMenuScreen_img.gameObject.SetActive(false);
            LevelEventManager.onResumedGame();
        });
    }

    public void ReturnMenu_Button()
    {
        curtain.gameObject.SetActive(true);
        curtain.DOColor(new Color(0f, 0f, 0f, 1f), .35f).OnComplete(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    public void Retry_Button()
    {
        // SHOW REWARD AD
        curtain.gameObject.SetActive(true);
        curtain.DOColor(new Color(0f, 0f, 0f, 1f), .35f).OnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    public void NextLevel_Button()
    {
        curtain.gameObject.SetActive(true);
        curtain.DOColor(new Color(0f, 0f, 0f, 1f), .35f).OnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
    #region Game End State Functions
    void NoTimeLeft_GameOverScreen()
    {
        GameOverScreen_img.gameObject.SetActive(true);
        gameOverReasonText.text = "NO TIME LEFT";
        gameOverStarValueText.text = LevelEconomyManager.instance.levelStar.ToString();
        GameOverScreen_img.DOColor(new Color(.13f, .11f, .13f, .85f), 1f).SetEase(Ease.OutCubic);
        gameOverReasonText.rectTransform.DOLocalMoveX(0f, .5f).SetEase(Ease.OutCirc);

        retryButton.transform.DOScale(1.1f, .75f).
        SetEase(Ease.InOutSine).
        SetDelay(.25f).
        OnComplete(() =>
        {
            retryButton.transform.DOScale(1f, .75f).
            SetDelay(.25f).
            SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        });

        GameOverPanel.DOLocalMoveX(0f, .5f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            gameOverStarIMG.rectTransform.DORotate(new Vector3(45f, 0f, 15f), 1f, RotateMode.Fast).
            SetEase(Ease.InOutSine).
            OnComplete(() =>
            {
                gameOverStarIMG.rectTransform.DORotate(new Vector3(45f, 0f, -15f), 1f, RotateMode.Fast).
                SetEase(Ease.InOutSine).
                SetLoops(-1, LoopType.Yoyo);
            });
            GameOverScreen_img.DOColor(new Color(.13f, .11f, .13f, 1f), 5f);
        });
    }

    void NoLife_GameOverScreen()
    {
        GameOverScreen_img.gameObject.SetActive(true);
        gameOverReasonText.text = "GAME OVER";
        gameOverStarValueText.text = LevelEconomyManager.instance.levelStar.ToString();
        GameOverScreen_img.DOColor(new Color(.13f, .11f, .13f, .85f), 1f).SetEase(Ease.OutCubic);
        gameOverReasonText.rectTransform.DOLocalMoveX(0f, .5f).SetEase(Ease.OutCirc);

        retryButton.transform.DOScale(1.1f, .75f).
        SetEase(Ease.InOutSine).
        SetDelay(.25f).
        OnComplete(() =>
        {
            retryButton.transform.DOScale(1f, .75f).
            SetDelay(.25f).
            SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        });

        GameOverPanel.DOLocalMoveX(0f, .5f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            gameOverStarIMG.rectTransform.DORotate(new Vector3(45f, 0f, 15f), 1f, RotateMode.Fast).
            SetEase(Ease.InOutSine).
            OnComplete(() =>
            {
                gameOverStarIMG.rectTransform.DORotate(new Vector3(45f, 0f, -15f), 1f, RotateMode.Fast).
                SetEase(Ease.InOutSine).
                SetLoops(-1, LoopType.Yoyo);
            });
            GameOverScreen_img.DOColor(new Color(.13f, .11f, .13f, 1f), 5f);
        });
    }

    void LevelPassed_Screen()
    {
        LevelPassedScreen_img.gameObject.SetActive(true);
        levelPassedText.text = "LEVEL PASSED";
        levelPassedStarValueText.text = LevelEconomyManager.instance.levelStar.ToString();
        LevelPassedScreen_img.DOColor(new Color(.13f, .11f, .13f, .85f), 1f).SetEase(Ease.OutCubic);
        levelPassedText.rectTransform.DOLocalMoveX(0f, .5f).SetEase(Ease.OutCirc);

        nextLevelButton.transform.DOScale(1.1f, .75f).
        SetEase(Ease.InOutSine).
        SetDelay(.25f).
        OnComplete(() =>
        {
            nextLevelButton.transform.DOScale(1f, .75f).
            SetDelay(.25f).
            SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        });

        LevelPassedPanel.DOLocalMoveX(0f, .5f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            levelPassedStarIMG.rectTransform.DORotate(new Vector3(45f, 0f, 15f), 1f, RotateMode.Fast).
            SetEase(Ease.InOutSine).
            OnComplete(() =>
            {
                levelPassedStarIMG.rectTransform.DORotate(new Vector3(45f, 0f, -15f), 1f, RotateMode.Fast).
                SetEase(Ease.InOutSine).
                SetLoops(-1, LoopType.Yoyo);
            });
            LevelPassedScreen_img.DOColor(new Color(.13f, .11f, .13f, 1f), 5f);
        });
    }
    #endregion

}
