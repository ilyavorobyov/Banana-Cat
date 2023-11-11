using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class BananaCatMover : MonoBehaviour
{
    [SerializeField] private AudioSource _crySound;
    [SerializeField] private AudioSource _happySound;
    [SerializeField] private AudioSource _jumpSound;
    [SerializeField] private Button _jumpButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedChanger;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speedBoostValue;

    private const string RunAnimationName = "Run";
    private const string IdleAnimationName = "Idle";
    private const string CryAnimationName = "Cry";
    private const string HappyAnimationName = "Happy";
    private const string JumpAnimationName = "Jump";
    private const string FruitTakeAnimationName = "FruitTake";

    private PlayerInput _playerInput;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _isCanMove = false;
    private bool _isCanJump = true;
    private float _moveDirection;
    private float _leftMoveDirection = 1;
    private float _stopMoveDirection = 0;
    private float _tempSpeed;
    private bool _isMobile = false;
    private bool _isSpeedAdded = false;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += ctx => OnChangeMoveDirection();
        _playerInput.Player.Jump.performed += ctx => OnJump();
        _playerInput.Player.Move.canceled += ctx => OnStopMovement();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _tempSpeed = _speed;
        OnBecomeHappy();
    }

    private void Update()
    {
        if (_isCanMove)
            transform.Translate(Vector3.right * _speed * _moveDirection * Time.deltaTime);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        BananaCatCollisionHandler.OpenGameOverPanelEvent += OnCry;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnCry;
        BananaCatCollisionHandler.FruitTakenEvent += OnFruitTake;
        BananaCatCollisionHandler.TookSpeedBoosterEvent += OnAddSpeedBoost;
        SpeedBoosterManager.StopAddSpeedEvent += OnRemoveSpeedBoost;
        Ground.GroundCollisionEvent += OnGroundCollision;
        BananaCat.SpeedChangedEvent += OnChangeSpeed;
        GameUI.ChangeGameStateEvent += OnChangeGameState;
        GameUI.StartGameEvent += OnSetStartSpeed;
        GameUI.ReviveEvent += OnSetStartSpeed;
        GameUI.GoToMenuEvent += OnBecomeHappy;
        TouchControlButton.StopMoveEvent += OnStopMovement;
        TouchControlButton.DirectionChangeEvent += Touch;
        _jumpButton.onClick.AddListener(OnJump);
        YandexGame.GetDataEvent += OnCheckMobileDevice;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnCry;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnCry;
        BananaCatCollisionHandler.FruitTakenEvent -= OnFruitTake;
        BananaCatCollisionHandler.TookSpeedBoosterEvent -= OnAddSpeedBoost;
        SpeedBoosterManager.StopAddSpeedEvent -= OnRemoveSpeedBoost;
        Ground.GroundCollisionEvent -= OnGroundCollision;
        BananaCat.SpeedChangedEvent -= OnChangeSpeed;
        GameUI.ChangeGameStateEvent -= OnChangeGameState;
        GameUI.StartGameEvent -= OnSetStartSpeed;
        GameUI.ReviveEvent -= OnSetStartSpeed;
        GameUI.GoToMenuEvent -= OnBecomeHappy;
        TouchControlButton.StopMoveEvent -= OnStopMovement;
        TouchControlButton.DirectionChangeEvent -= Touch;
        _jumpButton.onClick.RemoveListener(OnJump);
        YandexGame.GetDataEvent -= OnCheckMobileDevice;
    }

    public void OnBecomeHappy()
    {
        _animator.SetTrigger(HappyAnimationName);
    }

    private void OnChangeGameState(bool isPlaying)
    {
        _isCanMove = isPlaying;
        _crySound.Stop();
        OnStopMovement();

        if (isPlaying)
            _animator.SetTrigger(IdleAnimationName);
        else
            _animator.SetTrigger(HappyAnimationName);
    }

    private void OnCheckMobileDevice()
    {
        if (YandexGame.EnvironmentData.isMobile)
        _isMobile = true;
    }

    private void OnSetStartSpeed()
    {
        _speed = _tempSpeed;
    }

    private void OnCry()
    {
        _isCanMove = false;
        _animator.SetTrigger(CryAnimationName);
        _crySound.PlayDelayed(0);
    }

    private void OnFruitTake()
    {
        if (_isCanMove)
            _animator.SetTrigger(FruitTakeAnimationName);
    }

    private void OnChangeSpeed(bool isIncreased)
    {
        if (!isIncreased)
            _speed -= _speedChanger;
        else
            _speed += _speedChanger;
    }

    private void OnChangeMoveDirection()
    {
        if (_isCanMove)
        {
            _animator.SetTrigger(RunAnimationName);

            if (!_isMobile)
                _moveDirection = _playerInput.Player.Move.ReadValue<float>();

            if (_moveDirection != _stopMoveDirection)
            {
                if (_moveDirection == _leftMoveDirection)
                    _spriteRenderer.flipX = false;
                else
                    _spriteRenderer.flipX = true;
            }
        }
    }

    private void Touch(float Dir)
    {
        _moveDirection = Dir;
        OnChangeMoveDirection();
    }

    private void OnJump()
    {
        if (_isCanJump && _isCanMove)
        {
            _animator.SetTrigger(JumpAnimationName);
            _jumpSound.PlayDelayed(0);
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _rigidbody.velocity = Vector2.zero;
            _isCanJump = false;
        }
    }

    private void OnGroundCollision()
    {
        _isCanJump = true;
    }

    private void OnStopMovement()
    {
        if (_isCanMove)
        {
            _animator.SetTrigger(IdleAnimationName);
            _moveDirection = _stopMoveDirection;
        }
    }

    private void OnAddSpeedBoost()
    {
        if(!_isSpeedAdded)
        {
            _speed += _speedBoostValue;
            _isSpeedAdded = true;
        }
    }

    private void OnRemoveSpeedBoost()
    {
        _speed -= _speedBoostValue;
        _isSpeedAdded = false;
    }
}