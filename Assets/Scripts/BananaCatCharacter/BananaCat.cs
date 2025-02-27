using System;
using TMPro;
using UI;
using UnityEngine;

namespace BananaCatCharacter
{
    public class BananaCat : MonoBehaviour
    {
        [SerializeField] private UIElementsAnimation _uIElementsAnimation;
        [SerializeField] private AudioSource _fitSound;
        [SerializeField] private AudioSource _addScaleSound;
        [SerializeField] private TMP_Text _helpFitText;

        private Vector3 _scaleChange = new Vector3(0.25f, 0f, 0f);
        private Vector3 _startScale = Vector3.one;
        private int _fruitsNumberForFatDecrease = 3;
        private int _fruitEatenCounter = 0;
        private float _delayFitSound = 0.9f;

        public static Action<bool> SpeedChangedEvent;

        public int FatLevel { get; private set; }

        private void OnEnable()
        {
            GameUI.StartGameEvent += OnStartGameState;
            BananaCatCollisionHandler.BadFoodTakenEvent += OnBadFoodTaken;
            BananaCatCollisionHandler.FruitTakenEvent += OnFruitTaken;
        }

        private void OnDisable()
        {
            GameUI.StartGameEvent -= OnStartGameState;
            BananaCatCollisionHandler.BadFoodTakenEvent -= OnBadFoodTaken;
            BananaCatCollisionHandler.FruitTakenEvent -= OnFruitTaken;
        }

        private void OnStartGameState()
        {
            FatLevel = 0;
            _fruitEatenCounter = 0;
            transform.localScale = _startScale;
        }

        private void OnBadFoodTaken()
        {
            if (FatLevel == 0)
                _uIElementsAnimation.Appear(_helpFitText.gameObject);

            FatLevel++;
            transform.localScale += _scaleChange;
            _addScaleSound.PlayDelayed(0);
            SpeedChangedEvent?.Invoke(false);
            _fruitEatenCounter = 0;
        }

        private void OnFruitTaken()
        {
            if (FatLevel > 0)
            {
                _fruitEatenCounter++;
            }

            if (_fruitEatenCounter == _fruitsNumberForFatDecrease)
            {
                FatLevel--;
                _fruitEatenCounter = 0;
                transform.localScale -= _scaleChange;
                _fitSound.PlayDelayed(_delayFitSound);
                SpeedChangedEvent?.Invoke(true);

                if (FatLevel == 0)
                {
                    _uIElementsAnimation.Disappear(_helpFitText.gameObject);
                }
            }
        }
    }
}