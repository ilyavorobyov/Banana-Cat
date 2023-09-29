using System;
using UnityEngine;

public class MissedFruitsCounter : MonoBehaviour
{
    [SerializeField] private AudioSource _missingSound;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private MissingFruitMark _missingFruitMark;

    private int _fallenFruitsNumber;

    public static Action MaxFruitsNumberDroppedEvent;

    private void OnEnable()
    {
        Ground.FruitCollisionEvent += OnFruitDropped;
        GameUI.StartGameEvent += OnStartGame;
        GameUI.ReviveEvent += OnStartGame;
    }

    private void OnDisable()
    {
        Ground.FruitCollisionEvent -= OnFruitDropped;
        GameUI.StartGameEvent -= OnStartGame;
        GameUI.ReviveEvent -= OnStartGame;
    }

    private void OnStartGame()
    {
        _fallenFruitsNumber = 0;
    }

    private void OnFruitDropped(Vector3 position)
    {
        _missingSound.PlayDelayed(0);
        Instantiate(_missingFruitMark, position, Quaternion.identity);
        _fallenFruitsNumber++;
        _healthBar.TakePoint();

        if(_fallenFruitsNumber >= _healthBar.HealthPoints)
            MaxFruitsNumberDroppedEvent?.Invoke();
    }
}