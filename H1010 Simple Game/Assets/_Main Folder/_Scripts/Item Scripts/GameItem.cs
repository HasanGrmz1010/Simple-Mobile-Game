using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameItem : MonoBehaviour, ISelectable
{
    bool interactable;
    bool used;
    Rigidbody rgb;
    public string item_tag;

    private void OnEnable()
    {
        LevelEventManager.onTimesUp += GameOver_Behaviour;
        LevelEventManager.onNoLifeRemains += GameOver_Behaviour;
        LevelEventManager.onPausedGame += GamePaused_Behaviour;
        LevelEventManager.onResumedGame += GameResume_Behaviour;
    }

    private void OnDisable()
    {
        LevelEventManager.onTimesUp -= GameOver_Behaviour;
        LevelEventManager.onNoLifeRemains -= GameOver_Behaviour;
        LevelEventManager.onPausedGame -= GamePaused_Behaviour;
        LevelEventManager.onResumedGame -= GameResume_Behaviour;
    }

    private void Start()
    {
        used = false;
        interactable = true;
        rgb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "floor" && !LevelEventManager.instance.ableToSelect)
        {
            LevelEventManager.instance.ableToSelect = true;
        }
    }

    public void TouchStart()
    {
        if (BoxManager.instance.currentBox != null &&
            LevelEventManager.instance.ableToSelect &&
            interactable &&
            !used)
        {
            transform.DOScale(1.1f, .25f).SetEase(Ease.OutCirc);
        }
    }

    public void TouchHold()
    {

    }

    public void TouchRelease()
    {
        if (BoxManager.instance.currentBox != null &&
            LevelEventManager.instance.ableToSelect &&
            interactable &&
            !used)
        {
            Transform currentBox = BoxManager.instance.currentBox;
            interactable = true;
            used = true;
            rgb.useGravity = false;
            transform.DOMoveY(transform.position.y + 1f, .25f).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                transform.DOScale(.6f, .25f).SetEase(Ease.OutCirc);
                transform.DOMove(currentBox.GetChild(1).position, .2f).SetEase(Ease.OutCirc).OnComplete(() =>
                {
                    rgb.useGravity = true;
                    currentBox.GetComponent<Box>().AddItemToBox(this);
                    transform.SetParent(currentBox);
                });
            });
        }
    }

    // Scales to 1.1 and deactivates item (pop)
    void GameOver_Behaviour()
    {
        transform.DOScale(1.25f, .2f).SetEase(Ease.InOutQuad).SetDelay(Random.Range(1.5f, 6f)).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    void GamePaused_Behaviour()
    {
        interactable = false;
    }

    void GameResume_Behaviour()
    {
        interactable = true;
    }
}
