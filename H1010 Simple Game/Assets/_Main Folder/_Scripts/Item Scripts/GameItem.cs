using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameItem : MonoBehaviour, ISelectable
{
    Rigidbody rgb;
    public string item_tag;

    private void OnEnable()
    {
        LevelEventManager.onTimesUp += GameOver_Behaviour;
    }

    private void OnDisable()
    {
        LevelEventManager.onTimesUp -= GameOver_Behaviour;
    }

    private void Start()
    {
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
        if (BoxManager.instance.currentBox != null && LevelEventManager.instance.ableToSelect)
        {
            transform.DOScale(1.1f, .25f).SetEase(Ease.OutCirc);
        }
    }

    public void TouchHold()
    {

    }

    public void TouchRelease()
    {
        if (BoxManager.instance.currentBox != null && LevelEventManager.instance.ableToSelect)
        {
            Transform currentBox = BoxManager.instance.currentBox;
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

    void GameOver_Behaviour()
    {
        transform.DOScale(1.25f, .2f).SetEase(Ease.InOutQuad).SetDelay(Random.Range(1.5f, 6f)).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
