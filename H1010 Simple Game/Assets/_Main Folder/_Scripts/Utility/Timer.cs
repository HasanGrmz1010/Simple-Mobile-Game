using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    [SerializeField] Ease timerEase;

    bool smallerThan10;

    public float totalTime;
    private float currentTime;

    public RectTransform timeIndicator;
    public Image watch_img;
    public TextMeshProUGUI timerText;

    void Start()
    {
        smallerThan10 = false;
        currentTime = totalTime;
    }

    void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 10f && !smallerThan10)
            {
                NoTimeLeft_Tween();
            }

            if (currentTime <= 0f)
            {
                GameManager.instance.state = GameManager.GameState.timeUp_GameOver;
                LevelEventManager.onTimesUp();
                currentTime = 0f;
                this.transform.DOScale(.01f, .4f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                });
            }
            UpdateTimerDisplay();
        }
        
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void NoTimeLeft_Tween()
    {
        smallerThan10 = true;
        timeIndicator.DOScale(1.1f, .5f).SetEase(timerEase).OnComplete(() =>
        {
            
            timeIndicator.DOScale(1f, .5f).SetEase(timerEase).SetLoops(-1, LoopType.Yoyo);
            
        });

        watch_img.DOColor(Color.red, 1f).OnComplete(() =>
        {
            watch_img.DOColor(Color.white, 1f).SetLoops(-1);
        });
    }
}