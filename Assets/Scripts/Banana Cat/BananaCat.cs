using System;
using UnityEngine;

public class BananaCat : MonoBehaviour
{
    [SerializeField] private AudioSource _fitSound;
    [SerializeField] private AudioSource _addScaleSound;

    private Vector3 _scaleChange = new Vector3(0.25f, 0f, 0f);
    private int _fruitsNumberForFatDecrease = 3;
    private int _fatLevel = 0;
    private int _fruitEatenCounter = 0;
    private float _delayFitSound = 0.9f;

    public static Action<bool> SpeedChangedEvent;

    private void OnEnable()
    {
        BananaCatCollisionHandler.BadFoodTakenEvent += OnBadFoodTaken;
        BananaCatCollisionHandler.FruitTakenEvent += OnFruitTaken;
        BananaCatCollisionHandler.GameOverEvent += Die;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.BadFoodTakenEvent -= OnBadFoodTaken;
        BananaCatCollisionHandler.FruitTakenEvent -= OnFruitTaken;
        BananaCatCollisionHandler.GameOverEvent -= Die;
    }

    private void Die()
    {
        Debug.Log("die");
    }

    private void OnBadFoodTaken()
    {
        _fatLevel++;
        transform.localScale += _scaleChange;
        _addScaleSound.PlayDelayed(0);
        SpeedChangedEvent?.Invoke(false);
    }

    private void OnFruitTaken()
    {
        if (_fatLevel > 0)
            _fruitEatenCounter++;

        if (_fruitEatenCounter == _fruitsNumberForFatDecrease)
        {
            _fatLevel--;
            _fruitEatenCounter = 0;
            transform.localScale -= _scaleChange;
            _fitSound.PlayDelayed(_delayFitSound);
            SpeedChangedEvent?.Invoke(true);
        }
    }
}