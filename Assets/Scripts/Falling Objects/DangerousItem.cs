using UnityEngine;

public class DangerousItem : FallingObject
{
    public override void Die(BananaCat bananaCat)
    {
        bananaCat.Die();
    }
}
