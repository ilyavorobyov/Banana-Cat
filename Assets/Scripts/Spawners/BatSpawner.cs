using System.Collections;
using BananaCatCharacter;
using CameraBehavior;
using Enemies;
using MissedFruits;
using Random = UnityEngine.Random;
using UI;
using UnityEngine;

namespace Spawners
{
    public class BatSpawner : MonoBehaviour
    {
        [SerializeField] private BananaCat _bananaCat;
        [SerializeField] private float _minSpawnTime;
        [SerializeField] private float _maxSpawnTime;
        [SerializeField] private float _warningInfoDuration;
        [SerializeField] private Warning _warningSample;
        [SerializeField] private BatEnemy _batEnemySample;
        [SerializeField] private AudioSource _warningInfoSound;

        private Warning _warning;
        private BatEnemy _batEnemy;
        private float _leftEdge;
        private float _rightEdge;
        private float _maxYBananCatPosition;
        private float _bananCatYPosition;
        private float _addingToYPosition = 0.2f;
        private float _addingSpawnPosition = 0.5f;
        private float _zPosition = 0;
        private Vector3 _warningSpawnPosition;
        private Vector3 _batSpawnPosition;
        private Coroutine _createBat;

        private void Awake()
        {
            _warning = Instantiate(_warningSample, transform.position, Quaternion.identity);
            _warning.gameObject.SetActive(false);
            _batEnemy = Instantiate(_batEnemySample, transform.position, Quaternion.identity);
            _batEnemy.gameObject.SetActive(false);
            _bananCatYPosition = _bananaCat.gameObject.transform.position.y;
            _maxYBananCatPosition = _bananCatYPosition + _addingToYPosition;
        }

        private void OnEnable()
        {
            ScreenEdge.SetSpawnPositionsForBatSpawner += OnInit;
            BananaCatCollisionHandler.OpenGameOverPanelEvent += OnStopCreateObjects;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnStopCreateObjects;
            GameUI.StartGameEvent += OnBeginCreateObjects;
            GameUI.ReviveEvent += OnBeginCreateObjects;
            GameUI.GoToMenuEvent += OnStopCreateObjects;
        }

        private void OnDisable()
        {
            ScreenEdge.SetSpawnPositionsForBatSpawner -= OnInit;
            BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnStopCreateObjects;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnStopCreateObjects;
            GameUI.StartGameEvent -= OnBeginCreateObjects;
            GameUI.ReviveEvent -= OnBeginCreateObjects;
            GameUI.GoToMenuEvent -= OnStopCreateObjects;
        }

        private void OnStopCreateObjects()
        {
            if (_createBat != null)
            {
                StopCoroutine(_createBat);
            }
        }
        private void OnBeginCreateObjects()
        {
            OnStopCreateObjects();
            _createBat = StartCoroutine(CreateBat());
        }

        private void OnInit(float leftEdge, float rightEdge)
        {
            _leftEdge = leftEdge;
            _rightEdge = rightEdge;
        }

        private bool ChooseLeftSide()
        {
            int minNumber = 0;
            int maxNumber = 2;
            int number = Random.Range(minNumber, maxNumber);
            return number > 0;
        }

        private float CalculateYPosition()
        {
            return Random.Range(_bananCatYPosition, _maxYBananCatPosition);
        }

        private IEnumerator CreateBat()
        {
            float timeOfNewSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
            var waitWarningInfo = new WaitForSeconds(_warningInfoDuration);
            var waitForSeconds = new WaitForSeconds(timeOfNewSpawn);
            bool isLeftSide;

            while (true)
            {
                yield return waitForSeconds;
                isLeftSide = ChooseLeftSide();

                if (!_batEnemy.gameObject.activeSelf)
                {
                    if (isLeftSide)
                    {
                        _warningSpawnPosition = new Vector3(_leftEdge + _addingSpawnPosition,
                            CalculateYPosition(), _zPosition);
                        _batSpawnPosition = new Vector3(_leftEdge, _warningSpawnPosition.y, _zPosition);
                    }
                    else
                    {
                        _warningSpawnPosition = new Vector3(_rightEdge - _addingSpawnPosition,
                            CalculateYPosition(), _zPosition);
                        _batSpawnPosition = new Vector3(_rightEdge, _warningSpawnPosition.y, _zPosition);
                    }

                    _warningInfoSound.PlayDelayed(0);
                    _warning.transform.position = _warningSpawnPosition;
                    _warning.ShowWarningState(true);
                    yield return waitWarningInfo;
                    _warning.ShowWarningState(false);
                    _batEnemy.SetSpawnState(_batSpawnPosition, isLeftSide);
                    timeOfNewSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
                }
            }
        }
    }
}