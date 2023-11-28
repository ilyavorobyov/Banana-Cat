using System.Collections;
using UnityEngine;

public class CannonEnemySpawner : MonoBehaviour
{
    [SerializeField] private CannonEnemy _cannonEnemy;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private float _minCannonAttackDuration;
    [SerializeField] private float _maxCannonAttackDuration;

    private float _newAttackTime;
    private float _cannonAttackDuration;
    private Coroutine _cannonAttack;

    private void OnEnable()
    {
        BananaCatCollisionHandler.OpenGameOverPanelEvent += OnStopCannonAttack;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnStopCannonAttack;
        GameUI.StartGameEvent += OnLaunchCannonAttack;
        GameUI.ReviveEvent += OnLaunchCannonAttack;
        GameUI.GoToMenuEvent += OnStopCannonAttack;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnStopCannonAttack;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnStopCannonAttack;
        GameUI.StartGameEvent -= OnLaunchCannonAttack;
        GameUI.ReviveEvent -= OnLaunchCannonAttack;
        GameUI.GoToMenuEvent -= OnStopCannonAttack;
    }

    private void OnLaunchCannonAttack()
    {
        OnStopCannonAttack();
        _cannonAttack = StartCoroutine(CreateObjects());
    }

    private void OnStopCannonAttack()
    {
        if (_cannonAttack != null)
            StopCoroutine(_cannonAttack);
    }

    private IEnumerator CreateObjects()
    {
        _newAttackTime = Random.Range(_minSpawnTime, _maxSpawnTime);
        _cannonAttackDuration = Random.Range(_minCannonAttackDuration, _maxCannonAttackDuration);

        var waitForSeconds = new WaitForSeconds(_newAttackTime);
        var waitForCannonAttackDuration = new WaitForSeconds(_cannonAttackDuration);

        while (true)
        {
            yield return waitForSeconds;

            if (!_cannonEnemy.IsAttacks)
            {
                _cannonEnemy.RollIn();
                yield return waitForCannonAttackDuration;
                _cannonEnemy.RollOut();
                _newAttackTime = Random.Range(_minSpawnTime, _maxSpawnTime);
                _cannonAttackDuration = Random.Range(_minCannonAttackDuration, _maxCannonAttackDuration);
            }
        }
    }
}