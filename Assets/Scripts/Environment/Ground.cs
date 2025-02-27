using System;
using BananaCatCharacter;
using Enemies;
using FallingObjects;
using UnityEngine;

namespace Environment
{
    public class Ground : MonoBehaviour
    {
        private Vector3 _addingPosition = new Vector3(0f, 0.15f, 0f);

        public static Action GroundCollisionEvent;
        public static Action<Vector3> FruitCollisionEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out FallingObject fallingObject))
            {
                if (fallingObject is FruitItem)
                {
                    fallingObject.OnHideObject();
                    FruitCollisionEvent?.Invoke(fallingObject.transform.position + _addingPosition);
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

            if (collision.TryGetComponent(out CannonBall cannonBall))
            {
                cannonBall.OnHide();
            }
        }
    }
}