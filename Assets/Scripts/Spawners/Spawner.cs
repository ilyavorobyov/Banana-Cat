using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private FallingObject _fallingObject;
    [SerializeField] private int _createdObjectsNumber;
    [SerializeField] private float _dropChance;
    [SerializeField] private float _minFallingSpeed;
    [SerializeField] private float _maxFallingSpeed;
    [SerializeField] private float _dropChanceMultiplier;
    [SerializeField] private float _spawnTimeReducer;
    [SerializeField] private float _minTimeOfSpawn;
    [SerializeField] private float _maxTimeOfSpawn;

    protected FallingObject NextSpawnObject;
    protected Vector2 SpawnPosition;
    protected float MinXPosition;
    protected float MaxXPosition;
    protected float XPosition;
    protected float MaxXPositionValueChanger = 6;
    protected float MinXPositionValueChanger = 1;
    protected float ReductionMultiplier = 0.9f;
    protected float YSpawnPosition;
    protected float LastXPosition;

    private List<FallingObject> FallingObjects = new List<FallingObject>();
    private List<FallingObject> _hiddenObjects = new List<FallingObject>();
    private float _minChanceIncreaseNumber = 1.05f;
    private float _minSpawnTimeReducer = 0.5f;
    private int _maxDropChance = 101;
    private int _minDropChance = 10;
    private float _timeOfSpawn;
    private float _tempMaxTimeOfSpawn;
    private float _tempDrop—hance;

    private Coroutine _createObject;

    private void Awake()
    {
        for (int i = 0; i < _createdObjectsNumber; i++)
        {
            var fallingObject = Instantiate(_fallingObject, transform.position, Quaternion.identity);
            fallingObject.OnHide();
            FallingObjects.Add(fallingObject);
        }
    }

    private void Start()
    {
        _tempMaxTimeOfSpawn = _maxTimeOfSpawn;
        _tempDrop—hance = _dropChance;
    }

    private void OnEnable()
    {
        ScreenEdge.SetSpawnPositions += OnInit;
        BananaCatCollisionHandler.GameOverEvent += StopCreateObjects;
        GameUI.StartGameEvent += BeginCreateObjects;
        GameUI.TransitionToMenuEvent += StopCreateObjects;
        ScoreManager.AddDifficultyEvent += AddDifficulty;
    }

    private void OnDisable()
    {
        ScreenEdge.SetSpawnPositions -= OnInit;
        BananaCatCollisionHandler.GameOverEvent -= StopCreateObjects;
        GameUI.StartGameEvent -= BeginCreateObjects;
        GameUI.TransitionToMenuEvent -= StopCreateObjects;
        ScoreManager.AddDifficultyEvent -= AddDifficulty;
    }

    private void OnValidate()
    {
        if (_dropChance > _maxDropChance)
            _dropChance = _maxDropChance;

        if (_dropChance < _minDropChance)
            _dropChance += _minDropChance;

        if (_maxFallingSpeed < _minFallingSpeed)
            _maxFallingSpeed = _minFallingSpeed;

        if (_dropChanceMultiplier < _minChanceIncreaseNumber)
            _dropChanceMultiplier = _minChanceIncreaseNumber;

        if (_spawnTimeReducer < _minSpawnTimeReducer)
            _spawnTimeReducer = _minSpawnTimeReducer;
    }

    protected void AddDropChance()
    {
        _dropChance *= _dropChanceMultiplier;
    }

    protected void ReduceMaxSpawnTime()
    {
        if (_maxTimeOfSpawn - _spawnTimeReducer > _minTimeOfSpawn)
            _maxTimeOfSpawn -= _spawnTimeReducer;
    }

    private bool IsCanDropObject()
    {
        int randomNumber = Random.Range(_minDropChance, _maxDropChance);
        return randomNumber <= _dropChance;
    }

    private bool IsCanCollectHiddenObjects()
    {
        foreach (var fallingObject in FallingObjects)
        {
            if (fallingObject.gameObject.activeSelf == false)
                _hiddenObjects.Add(fallingObject);
        }

        if (_hiddenObjects.Count > 0)
        {
            NextSpawnObject = _hiddenObjects[Random.Range(0, _hiddenObjects.Count)];
            _hiddenObjects.Clear();
            return true;
        }
        else
            return false;
    }

    private float CalculateFallingSpeed()
    {
        return Random.Range(_minFallingSpeed, _maxFallingSpeed);
    }

    private void OnInit(float minXPosition, float maxXPosition, float ySpawnPosition)
    {
        MinXPosition = minXPosition;
        MaxXPosition = maxXPosition;
        YSpawnPosition = ySpawnPosition;
    }

    private void TryDropObject()
    {
        if (IsCanDropObject() && IsCanCollectHiddenObjects())
        {
            XPosition = CalculateNewXPosition(LastXPosition);
            SpawnPosition = new Vector2(XPosition, YSpawnPosition);
            NextSpawnObject.Init(CalculateFallingSpeed(), SpawnPosition);
        }
    }

    private float CalculateNewXPosition(float xSpawnPosition)
    {
        float newPositionX = Random.Range(MinXPosition, MaxXPosition);

        while (Mathf.Floor(xSpawnPosition) == Mathf.Floor(newPositionX))
            newPositionX = Random.Range(MinXPosition, MaxXPosition);

        return newPositionX;
    }

    private void BeginCreateObjects()
    {
        _dropChance = _tempDrop—hance;
        _maxTimeOfSpawn = _tempMaxTimeOfSpawn;
        StopCreateObjects();
        _createObject = StartCoroutine(CreateObjects2());
    }

    private void StopCreateObjects()
    {
        if (_createObject != null)
        {
            StopCoroutine(_createObject);
        }
    }

    private IEnumerator CreateObjects2()
    {
        _timeOfSpawn = Random.Range(_minTimeOfSpawn, _maxTimeOfSpawn);

        var waitForSeconds = new WaitForSeconds(_timeOfSpawn);

        while (true)
        {
            yield return waitForSeconds;

            if (IsCanCollectHiddenObjects())
            {
                SpawnPosition = new Vector2(Random.Range(MinXPosition, MaxXPosition), YSpawnPosition);
                XPosition = SpawnPosition.x;
                TryDropObject();
                _timeOfSpawn = Random.Range(_minTimeOfSpawn, _maxTimeOfSpawn);
                CalculateNewXPosition(XPosition);
            }
        }
    }

    public abstract void AddDifficulty();
}