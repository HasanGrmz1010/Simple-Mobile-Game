using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{

    public void TouchStart();
    public void TouchHold();
    public void TouchRelease();

}
