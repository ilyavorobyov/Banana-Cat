using System;
using UnityEngine;

[RequireComponent(typeof(BananaCatMover))]
public class BananaCatCollisionHandler : MonoBehaviour
{
    [SerializeField] private BananaCatHelmet _bananaCatHelmet;
    [SerializeField] private FruitSpawner _fruitSpawner;
    [SerializeField] private AudioSource _munchBadFoodSound;
    [SerializeField] private AudioSource _munchFruitSound;
    [SerializeField] private AudioSource _hitSound;

    private bool _isHelmetTurnOn = false;
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
                if(!_isHelmetTurnOn)
                {
                    _hitSound.PlayDelayed(0);
                    GameOverEvent?.Invoke();
                }
                else
                {
                    _bananaCatHelmet.ChangeVisability(false);
                    _isHelmetTurnOn = false;
                }
            }
            else if(fallingObject is NerfItem)
            {
                _munchBadFoodSound.PlayDelayed(0);
                BadFoodTakenEvent?.Invoke();
            }
            else if (fallingObject is Helmet)
            {
                _isHelmetTurnOn = true;
                _bananaCatHelmet.ChangeVisability(true);
            }

            fallingObject.OnHide();
        }
    }
}