using System.Collections.Generic;
using UnityEngine;

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
        BananaCatCollisionHandler.GameOverEvent += OnGameOver;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnGameOver;
        GameUI.StartGameEvent += OnStartGame;
        GameUI.TransitionToMenuEvent += OnGameOver;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.GameOverEvent -= OnGameOver;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnGameOver;
        GameUI.StartGameEvent -= OnStartGame;
        GameUI.TransitionToMenuEvent -= OnGameOver;
    }

    public void TakePoint()
    {
        _healthBarPoints[_missingPointsNumber].BecomeInactive();
        _missingPointsNumber++;
    }

    private void OnStartGame()
    {
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
            healthBarPoint.HideObject();
        }

        _healthBarPoints.Clear();
    }
}