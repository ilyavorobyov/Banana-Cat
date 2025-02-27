using System.Collections;
using System.Collections.Generic;
using BananaCatCharacter;
using CameraBehavior;
using DG.Tweening;
using MissedFruits;
using UI;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Animator))]
    public class CannonEnemy : MonoBehaviour
    {
        private const string HitAnimationName = "Hit";
        private const string ShotAnimationName = "Shot";

        [SerializeField] private Transform _shotPoint;
        [SerializeField] private BananaCat _bananaCat;
        [SerializeField] private CannonBall _cannonBallSample;
        [SerializeField] private float _moveAnimationDuration;
        [SerializeField] private AudioSource _appearSound;
        [SerializeField] private AudioSource _disappearSound;
        [SerializeField] private AudioSource _hitSound;
        [SerializeField] private AudioSource _shotSound;
        [SerializeField] private int _healthPoints;
        [SerializeField] private float _minShootTime;
        [SerializeField] private float _maxShootTime;

        private List<CannonBall> _cannonBalls = new List<CannonBall>();
        private int _createdCannonBallsNumber = 3;
        private Animator _animator;
        private float _upperEdge;
        private int _currentHealthPoints;
        private float _decreaseUpperEdge = 2f;
        private float _zPosition = 1f;
        private Vector3 _startPosition;
        private Coroutine _shoot;

        public bool IsAttacks { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            for (int i = 0; i < _createdCannonBallsNumber; i++)
            {
                var cannonBall = Instantiate(_cannonBallSample, transform.position, Quaternion.identity);
                cannonBall.OnHide();
                cannonBall.transform.position = gameObject.transform.position;
                _cannonBalls.Add(cannonBall);
            }
        }

        private void OnEnable()
        {
            ScreenEdge.SetSpawnPositionsForCannon += OnInit;
            BananaCatCollisionHandler.OpenGameOverPanelEvent += RollOut;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent += RollOut;
            GameUI.HideFallingObjects += RollOut;
            GameUI.GoToMenuEvent += RollOut;
        }

        private void OnDisable()
        {
            ScreenEdge.SetSpawnPositionsForCannon -= OnInit;
            BananaCatCollisionHandler.OpenGameOverPanelEvent -= RollOut;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= RollOut;
            GameUI.HideFallingObjects -= RollOut;
            GameUI.GoToMenuEvent -= RollOut;
        }

        private void OnMouseDown()
        {
            --_currentHealthPoints;
            _animator.SetTrigger(HitAnimationName);
            _hitSound.PlayDelayed(0);

            if (_currentHealthPoints <= 0)
                RollOut();
        }

        private void Update()
        {
            Vector3 dir = _bananaCat.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void RollIn()
        {
            _currentHealthPoints = _healthPoints;
            _appearSound.PlayDelayed(0);
            transform.DOMoveY(_upperEdge - _decreaseUpperEdge, _moveAnimationDuration).SetUpdate(true);
            IsAttacks = true;
            OnBeginCreateObjects();
        }

        public void RollOut()
        {
            if (IsAttacks)
            {
                _disappearSound.PlayDelayed(0);
                transform.DOMoveY(_upperEdge + _decreaseUpperEdge, _moveAnimationDuration).SetUpdate(true);
                IsAttacks = false;
                OnStopCreateObjects();
            }
        }

        private void OnStopCreateObjects()
        {
            if (_shoot != null)
            {
                StopCoroutine(_shoot);
            }
        }
        private void OnBeginCreateObjects()
        {
            OnStopCreateObjects();
            _shoot = StartCoroutine(Shoot());
        }

        private void OnInit(float upperEdge, float midpointX)
        {
            _upperEdge = upperEdge;
            _startPosition = new Vector3(midpointX, _upperEdge + _decreaseUpperEdge, _zPosition);
            transform.position = _startPosition;
        }

        private IEnumerator Shoot()
        {
            float timeOfNewShot = Random.Range(_minShootTime, _maxShootTime);
            var waitForSeconds = new WaitForSeconds(timeOfNewShot);

            while (true)
            {
                yield return waitForSeconds;

                foreach (var cannonBall in _cannonBalls)
                {
                    if (!cannonBall.gameObject.activeSelf)
                    {
                        _animator.SetTrigger(ShotAnimationName);
                        cannonBall.TurnOnObject();
                        cannonBall.transform.position = _shotPoint.transform.position;
                        cannonBall.transform.rotation = gameObject.transform.rotation;
                        _shotSound.PlayDelayed(0);
                        timeOfNewShot = Random.Range(_minShootTime, _maxShootTime);
                        break;
                    }
                }
            }
        }
    }
}