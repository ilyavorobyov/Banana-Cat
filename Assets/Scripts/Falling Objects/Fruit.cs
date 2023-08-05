using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fruit : FallingObject
{
    public override void Die(BananaCat bananaCat)
    {
        Hide();
    }
}
