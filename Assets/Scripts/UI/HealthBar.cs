using System.Collections.Generic;
using BananaCatCharacter;
using MissedFruits;
using UnityEngine;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private int _healthPoints;
        [SerializeField] private HealthBarPoint _healthBarPoint;

        private List<HealthBarPoint> _healthBarPoints = new List<HealthBarPoint>();
        private int _missingPointsNumber = 0;

        public int HealthPoints { get; private set; }

        private void Awake()
        {
            HealthPoints = _healthPoints;
        }

        private void OnEnable()
        {
            BananaCatCollisionHandler.OpenGameOverPanelEvent += OnGameOver;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnGameOver;
            GameUI.ReviveEvent += OnStartGame;
            GameUI.StartGameEvent += OnStartGame;
            GameUI.GoToMenuEvent += OnGameOver;
        }

        private void OnDisable()
        {
            BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnGameOver;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnGameOver;
            GameUI.StartGameEvent -= OnStartGame;
            GameUI.GoToMenuEvent -= OnGameOver;
            GameUI.StartGameEvent -= OnStartGame;
        }

        public void TakeLifePoint()
        {
            _healthBarPoints[_missingPointsNumber].BecomeInactive();
            _missingPointsNumber++;
        }

        private void OnStartGame()
        {
            OnGameOver();
            _missingPointsNumber = 0;

            for (int i = 0; i < HealthPoints; i++)
            {
                var healthBarPoint = Instantiate(_healthBarPoint, transform.position, Quaternion.identity);
                healthBarPoint.transform.SetParent(transform, false);
                _healthBarPoints.Add(healthBarPoint);
            }
        }

        private void OnGameOver()
        {
            foreach (var healthBarPoint in _healthBarPoints)
            {
                Destroy(healthBarPoint.gameObject);
            }

            _healthBarPoints.Clear();
        }
    }
}