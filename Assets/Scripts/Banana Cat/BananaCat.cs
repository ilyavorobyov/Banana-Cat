using System;
using TMPro;
using UnityEngine;

public class BananaCat : MonoBehaviour
{
    [SerializeField] private UIElementsAnimation _uIElementsAnimation;
    [SerializeField] private AudioSource _fitSound;
    [SerializeField] private AudioSource _addScaleSound;
    [SerializeField] private TMP_Text _helpFitText;

    private Vector3 _scaleChange = new Vector3(0.25f, 0f, 0f);
    private Vector3 _startScale = Vector3.one;
    private int _fruitsNumberForFatDecrease = 3;
    private int _fatLevel = 0;
    private int _fruitEatenCounter = 0;
    private float _delayFitSound = 0.9f;

    public static Action<bool> SpeedChangedEvent;

    private void OnEnable()
    {
        GameUI.ChangeGameStateEvent += OnChangeGameState;
        BananaCatCollisionHandler.BadFoodTakenEvent += OnBadFoodTaken;
        BananaCatCollisionHandler.FruitTakenEvent += OnFruitTaken;
    }

    private void OnDisable()
    {
        GameUI.ChangeGameStateEvent -= OnChangeGameState;
        BananaCatCollisionHandler.BadFoodTakenEvent -= OnBadFoodTaken;
        BananaCatCollisionHandler.FruitTakenEvent -= OnFruitTaken;
    }

    private void OnChangeGameState(bool isPlaying)
    {
        if (isPlaying)
        {
            _fatLevel = 0;
            _fruitEatenCounter = 0;
            transform.localScale = _startScale;
        }
    }

    private void OnBadFoodTaken()
    {
        if (_fatLevel == 0)
            _uIElementsAnimation.Appear(_helpFitText.gameObject);

        _fatLevel++;
        transform.localScale += _scaleChange;
        _addScaleSound.PlayDelayed(0);
        SpeedChangedEvent?.Invoke(false);
        _fruitEatenCounter = 0;
    }

    private void OnFruitTaken()
    {
        if (_fatLevel > 0)
        {
            _fruitEatenCounter++;
        }

        if (_fruitEatenCounter == _fruitsNumberForFatDecrease)
        {
            _fatLevel--;
            _fruitEatenCounter = 0;
            transform.localScale -= _scaleChange;
            _fitSound.PlayDelayed(_delayFitSound);
            SpeedChangedEvent?.Invoke(true);

            if (_fatLevel == 0)
            {
                _uIElementsAnimation.Disappear(_helpFitText.gameObject);
            }
        }
    }
}