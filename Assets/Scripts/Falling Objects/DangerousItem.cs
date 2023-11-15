using UnityEngine;

public class DangerousItem : FallingObject
{
    public override void OnMouseDown()
    {
        TapSound.PlayDelayed(0);
        OnHideObject();
    }
}