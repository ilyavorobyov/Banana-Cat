using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerfItem : FallingObject
{
    public override void OnMouseDown()
    {
        TapSound.PlayDelayed(0);
        OnHideObject();
    }
}