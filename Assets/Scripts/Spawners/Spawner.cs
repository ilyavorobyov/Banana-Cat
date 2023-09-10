using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private FallingObject _fallingObject;
    [SerializeField] private int _numberOfObjects;
    [SerializeField] private int _percentageDrop—hance;
    [SerializeField] private float _minDelayTime;
    [SerializeField] private float _maxDelayTime;
    [SerializeField] private float _minFallingSpeed;
    [SerializeField] private float _maxFallingSpeed;

    protected FallingObject NextSpawnObject;
    protected Vector2 SpawnPosition;
    protected float MinXPosition;
    protected float MaxXPosition;
    protected float XPosition;
    protected float MaxXPositionValueChanger = 6;
    protected float MinXPositionValueChanger = 1;
    protected float YSpawnPosition;
    protected float LastXPosition;

    private List<FallingObject> FallingObjects = new List<FallingObject>();
    private List<FallingObject> _hiddenObjects = new List<FallingObject>();
    private int _maxDropChance = 101;
    private int _minDropChance = 1;

    private void Awake()
    {
        for (int i = 0; i < _numberOfObjects; i++)
        {
            var fallingObject = Instantiate(_fallingObject, transform.position, Quaternion.identity);
            fallingObject.OnHide();
            FallingObjects.Add(fallingObject);
        }
    }

    private void OnEnable()
    {
        ScreenEdge.SetSpawnPositions += OnInit;
    }

    private void OnDisable()
    {
        ScreenEdge.SetSpawnPositions -= OnInit;
    }

    private void OnValidate()
    {
        if(_percentageDrop—hance > _maxDropChance)
            _percentageDrop—hance = _maxDropChance;

        if(_percentageDrop—hance < _minDropChance)
            _percentageDrop—hance += _minDropChance;

        if(_maxDelayTime < _minDelayTime)
            _maxDelayTime = _minDelayTime;

        if(_maxFallingSpeed < _minFallingSpeed)
            _maxFallingSpeed = _minFallingSpeed;
    }

    public void ShowItem(float xSpawnPosition)
    {
        LastXPosition = xSpawnPosition;
        Invoke(nameof(TryDropObject), CalculateDelayTime());
    }

    protected float CalculateDelayTime()
    {
        return Random.Range(_minDelayTime, _maxDelayTime);
    }

    protected bool IsCanDropObject()
    {
        int randomNumber = Random.Range(_minDropChance, _maxDropChance);
        return randomNumber <= _percentageDrop—hance;
    }

    protected bool IsCanCollectHiddenObjects()
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

    protected float CalculateFallingSpeed()
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
        
        while(Mathf.Floor(xSpawnPosition) == Mathf.Floor(newPositionX))
            newPositionX = Random.Range(MinXPosition, MaxXPosition);

        return newPositionX;
    }
}