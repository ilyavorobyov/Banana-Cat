using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BananaCatMover))]
public class BananaCatCollisionHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _munchBadFoodSound;
    [SerializeField] private AudioSource _munchFruitSound;
    [SerializeField] private AudioSource _hitSound;

    public static Action FruitTakenEvent;
    public static Action BadFoodTakenEvent;
    public static Action GameOverEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out FallingObject fallingObject))
        {
            if (fallingObject is FruitItem)
            {
                _munchFruitSound.PlayDelayed(0);
                FruitTakenEvent.Invoke();
            }
            else if(fallingObject is DangerousItem)
            {
                _hitSound.PlayDelayed(0);
                GameOverEvent?.Invoke();
            }
            else if(fallingObject is NerfItem)
            {
                _munchBadFoodSound.PlayDelayed(0);
                BadFoodTakenEvent?.Invoke();
            }

            fallingObject.OnHide();
        }
    }
}