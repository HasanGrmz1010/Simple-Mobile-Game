using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    #region Singleton
    public static TouchHandler instance;
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

    public LayerMask layerMask;

    private void Update()
    {
        HandleTouchInputs();
    }

    void HandleTouchInputs()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out hit, 9999f, layerMask))
                    {
                        if (hit.transform.GetComponent<GameItem>())
                        {
                            GameItem item = hit.transform.GetComponent<GameItem>();
                            item.TouchStart();
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    break;

                case TouchPhase.Stationary:
                    if (Physics.Raycast(ray, out hit, 9999f, layerMask))
                    {
                        if (hit.transform.GetComponent<GameItem>())
                        {
                            GameItem item = hit.transform.GetComponent<GameItem>();
                            item.TouchHold();
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    if (Physics.Raycast(ray, out hit, 9999f, layerMask))
                    {
                        if (hit.transform.GetComponent<GameItem>())
                        {
                            GameItem item = hit.transform.GetComponent<GameItem>();
                            item.TouchRelease();
                        }
                    }
                    break;

                case TouchPhase.Canceled:
                    break;

                default:
                    break;
            }
        }
    }
}
