using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class BatEnemy : MonoBehaviour 
{
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private AudioSource _dieSound;

    private const string DieAnimationName = "Die";

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private float _hideDuration = 0.25f;
    private Animator _animator;
    private float _stopSpeed = 0;
    private float _speed;
    private float _speedMultiplier = -1;
    private bool _isLeftSideAppearance;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }

    private void OnEnable()
    {
        BananaCatCollisionHandler.OpenGameOverPanelEvent += OnHide;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnHide;
        GameUI.HideFallingObjects += OnHide;
        GameUI.GoToMenuEvent += OnHide;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnHide;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnHide;
        GameUI.HideFallingObjects -= OnHide;
        GameUI.GoToMenuEvent -= OnHide;
    }

    private void OnMouseDown()
    {
        _dieSound.PlayDelayed(0);
        Die();
    }

    public void SetSpawnState(Vector3 position, bool isLeftSide)
    {
        _boxCollider2D.enabled = true;
        gameObject.transform.position = position;
        _isLeftSideAppearance = isLeftSide;
        gameObject.SetActive(true);
        SetSpeed();

        if(_isLeftSideAppearance)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
    }

    public void Die()
    {
        _boxCollider2D.enabled = false;
        _animator.SetTrigger(DieAnimationName);
        _speed = _stopSpeed;
        Invoke(nameof(OnHide), _hideDuration);
    }

    private void SetSpeed()
    {
        _speed = Random.Range(_minSpeed, _maxSpeed);

        if (!_isLeftSideAppearance)
            _speed *= _speedMultiplier;
    }

    private void OnHide()
    {
        gameObject.SetActive(false);
    }
}