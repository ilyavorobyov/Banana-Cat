using System;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public static Action GroundCollisionEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out FallingObject fallingObject))
        {
            if (fallingObject is FruitItem)
            {
                fallingObject.Hide();
            }
            else
            {
                fallingObject.Hide();
            }
        }

        if (collision.TryGetComponent(out BananaCat bananaCat))
        {
            GroundCollisionEvent?.Invoke();
        }
    }
}