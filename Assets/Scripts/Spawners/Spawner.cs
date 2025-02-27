using System.Collections;
using System.Collections.Generic;
using BananaCatCharacter;
using CameraBehavior;
using FallingObjects;
using MissedFruits;
using Random = UnityEngine.Random;
using UI;
using UnityEngine;

namespace Spawners
{
    public abstract class Spawner : MonoBehaviour
    {
        [SerializeField] private FallingObject _fallingObject;
        [SerializeField] private int _createdObjectsNumber;
        [SerializeField] private float _dropChance;
        [SerializeField] private float _minFallingSpeed;
        [SerializeField] private float _maxFallingSpeed;
        [SerializeField] private float _dropChanceMultiplier;
        [SerializeField] private float _spawnTimeReducer;
        [SerializeField] private float _minSpawnTime;
        [SerializeField] private float _maxSpawnTime;

        private List<FallingObject> _fallingObjects = new List<FallingObject>();
        private List<FallingObject> _hiddenObjects = new List<FallingObject>();
        private FallingObject _nextSpawnObject;
        private Vector2 _spawnPosition;
        private float _minXPosition;
        private float _maxXPosition;
        private float _xPosition;
        private float _ySpawnPosition;
        private float _lastXPosition;
        private float _minChanceIncreaseNumber = 1.02f;
        private float _minSpawnTimeReducer = 0.2f;
        private int _maxDropChance = 101;
        private int _minDropChance = 10;
        private float _timeOfNewSpawn;
        private float _tempMaxTimeOfSpawn;
        private float _tempDropChance;

        private Coroutine _createObject;

        private void Awake()
        {
            for (int i = 0; i < _createdObjectsNumber; i++)
            {
                var fallingObject = Instantiate(_fallingObject, transform.position, Quaternion.identity);
                _fallingObjects.Add(fallingObject);
            }
        }

        private void Start()
        {
            _tempMaxTimeOfSpawn = _maxSpawnTime;
            _tempDropChance = _dropChance;
        }

        private void OnEnable()
        {
            ScreenEdge.SetSpawnPositions += OnInit;
            BananaCatCollisionHandler.OpenGameOverPanelEvent += OnStopCreateObjects;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnStopCreateObjects;
            GameUI.StartGameEvent += OnBeginCreateObjects;
            GameUI.ReviveEvent += OnBeginCreateObjects;
            GameUI.GoToMenuEvent += OnStopCreateObjects;
            ScoreManager.AddDifficultyEvent += OnAddDifficulty;
        }

        private void OnDisable()
        {
            ScreenEdge.SetSpawnPositions -= OnInit;
            BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnStopCreateObjects;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnStopCreateObjects;
            GameUI.StartGameEvent -= OnBeginCreateObjects;
            GameUI.ReviveEvent -= OnBeginCreateObjects;
            GameUI.GoToMenuEvent -= OnStopCreateObjects;
            ScoreManager.AddDifficultyEvent -= OnAddDifficulty;
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
            if (_maxSpawnTime - _spawnTimeReducer > _minSpawnTime)
                _maxSpawnTime -= _spawnTimeReducer;
        }

        private bool IsCanDropObject()
        {
            int randomNumber = Random.Range(_minDropChance, _maxDropChance);
            return randomNumber <= _dropChance;
        }

        private bool IsCanCollectHiddenObjects()
        {
            foreach (var fallingObject in _fallingObjects)
            {
                if (fallingObject.gameObject.activeSelf == false)
                    _hiddenObjects.Add(fallingObject);
            }

            if (_hiddenObjects.Count > 0)
            {
                _nextSpawnObject = _hiddenObjects[Random.Range(0, _hiddenObjects.Count)];
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
            _minXPosition = minXPosition;
            _maxXPosition = maxXPosition;
            _ySpawnPosition = ySpawnPosition;
        }

        private void TryDropObject()
        {
            if (IsCanDropObject() && IsCanCollectHiddenObjects())
            {
                _xPosition = CalculateNewXPosition(_lastXPosition);
                _lastXPosition = _xPosition;
                _spawnPosition = new Vector2(_xPosition, _ySpawnPosition);
                _nextSpawnObject.Init(CalculateFallingSpeed(), _spawnPosition);
            }
        }

        private float CalculateNewXPosition(float xSpawnPosition)
        {
            float newPositionX = Random.Range(_minXPosition, _maxXPosition);

            while (Mathf.Floor(xSpawnPosition) == Mathf.Floor(newPositionX))
                newPositionX = Random.Range(_minXPosition, _maxXPosition);

            return newPositionX;
        }

        private void OnBeginCreateObjects()
        {
            _dropChance = _tempDropChance;
            _maxSpawnTime = _tempMaxTimeOfSpawn;
            OnStopCreateObjects();
            _createObject = StartCoroutine(CreateObjects());
        }

        private void OnStopCreateObjects()
        {
            if (_createObject != null)
            {
                StopCoroutine(_createObject);
            }
        }

        private IEnumerator CreateObjects()
        {
            _timeOfNewSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);

            var waitForSeconds = new WaitForSeconds(_timeOfNewSpawn);

            while (true)
            {
                yield return waitForSeconds;

                if (IsCanCollectHiddenObjects())
                {
                    _spawnPosition = new Vector2(Random.Range(_minXPosition, _maxXPosition), _ySpawnPosition);
                    _xPosition = _spawnPosition.x;
                    TryDropObject();
                    _timeOfNewSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
                    CalculateNewXPosition(_xPosition);
                }
            }
        }

        public abstract void OnAddDifficulty();
    }
}