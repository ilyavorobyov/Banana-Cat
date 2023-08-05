using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class BananaCatMover : MonoBehaviour
{
    [SerializeField] private AudioSource _crySound;
    [SerializeField] private AudioSource _happySound;
    [SerializeField] private float _speed;

    private const string RunAnimationName = "Run";
    private const string RunAnimationBoolName = "isRunning";
    private const string IdleAnimationName = "Idle";
    private const string CryAnimationName = "Cry";
    private const string HappyAnimationName = "Happy";

    private PlayerInput _playerInput;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _isCrying = false;
    private bool _isHappy = false;
    private bool _isCanMove = true;
    private float _moveDirection;
    private float _leftMoveDirection = 1;
    private float _stopMoveDirection = 0;

    public static Action<bool> StateChangeEvent;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += ctx => ReadMoveDirection();
        _playerInput.Player.Move.canceled += ctx => StopMovement();
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isCanMove)
        {
            transform.Translate(Vector3.right * _speed * _moveDirection * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void ReadMoveDirection()
    {
        if (_isCanMove)
        {
            _animator.SetTrigger(RunAnimationName);
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

    private void StopMovement()
    {
        _animator.SetTrigger(IdleAnimationName);
        _moveDirection = _stopMoveDirection;
    }

    public void Cry()
    {
        if (_isCrying)
        {
            _isCanMove = true;
            _animator.SetTrigger(IdleAnimationName);
            _isCrying = false;
            _crySound.Stop();
            StateChangeEvent?.Invoke(true);
            _isCanMove = true;
        }
        else
        {
            _isCanMove = false;
            _animator.SetTrigger(CryAnimationName);
            _isCrying = true;
            _crySound.PlayDelayed(0);
            StateChangeEvent?.Invoke(false);
            _isCanMove = false;

            if (_isHappy)
            {
                OnHappy();
            }
        }
    }

    private void OnHappy()
    {
        if (_isHappy)
        {
            _animator.SetTrigger(IdleAnimationName);
            _isHappy = false;
            _happySound.Stop();
            StateChangeEvent?.Invoke(true);
            _isCanMove = true;
        }
        else
        {
            _isCanMove = false;
            _animator.SetTrigger(HappyAnimationName);
            _isHappy = true;
            _happySound.PlayDelayed(0);
            StateChangeEvent?.Invoke(false);

            if (_isCrying)
            {
                Cry();
            }
        }
    }

}