using System;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public static Action GroundCollisionEvent;
    public static Action<Vector3> FruitCollisionEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out FallingObject fallingObject))
        {
            if (fallingObject is FruitItem)
            {
                fallingObject.OnHideObject();
                FruitCollisionEvent?.Invoke(fallingObject.transform.position);
            }
            else
            {
                fallingObject.OnHideObject();
            }
        }

        if (collision.TryGetComponent(out BananaCat bananaCat))
        {
            GroundCollisionEvent?.Invoke();
        }

        if(collision.TryGetComponent(out CannonBall cannonBall))
        {
            cannonBall.OnHide();
        }
    }
}