using System;
using UnityEngine;

[RequireComponent(typeof(BananaCatMover))]
public class BananaCatCollisionHandler : MonoBehaviour
{
    [SerializeField] private BananaCatHelmet _bananaCatHelmet;
    [SerializeField] private FruitSpawner _fruitSpawner;
    [SerializeField] private AudioSource _munchBadFoodSound;
    [SerializeField] private AudioSource _munchFruitSound;
    [SerializeField] private AudioSource _dieSound;
    [SerializeField] private AudioSource _helmetHitSound;
    [SerializeField] private AudioSource _takingHelmetSound;

    private bool _isHelmetTurnOn = false;

    public static Action FruitTakenEvent;
    public static Action BadFoodTakenEvent;
    public static Action OpenGameOverPanelEvent;

    private void OnEnable()
    {
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnTurnOffHelmet;
        GameUI.GoToMenuEvent += OnTurnOffHelmet;
    }

    private void OnDisable()
    {
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnTurnOffHelmet;
        GameUI.GoToMenuEvent -= OnTurnOffHelmet;
    }

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
                    _dieSound.PlayDelayed(0);
                    OpenGameOverPanelEvent?.Invoke();
                    OnTurnOffHelmet();
                }
                else
                {
                    _helmetHitSound.PlayDelayed(0);
                    OnTurnOffHelmet();
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
                _takingHelmetSound.PlayDelayed(0);
            }

            fallingObject.OnHideObject();
        }
    }

    private void OnTurnOffHelmet()
    {
        _bananaCatHelmet.ChangeVisability(false);
        _isHelmetTurnOn = false;
    }
}