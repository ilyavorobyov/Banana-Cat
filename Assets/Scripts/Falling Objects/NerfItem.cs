public class NerfItem : FallingObject
{
    public override void OnMouseDown()
    {
        TapSound.PlayDelayed(0);
        OnHideObject();
    }
}