using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class FallingObject : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private float _hideAnimationDuration = 0.25f;
    private float _minRotationSpeed = 1;
    private float _maxRotationSpeed = 3;
    private float _rotationSpeed;
    private float _speed;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseSprite();
    }

    private void Update()
    {
        transform.position += Vector3.down * _speed * Time.deltaTime;
        transform.Rotate(0, 0, _rotationSpeed);
    }

    private void OnEnable()
    {
        BananaCatCollisionHandler.GameOverEvent += OnHide;
        GameUI.HideFallingObjects += OnHide;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.GameOverEvent -= OnHide;
        GameUI.HideFallingObjects -= OnHide;
    }

    public void Init(float speed, Vector2 position)
    {
        transform.localScale = Vector3.one;
        _boxCollider.enabled = true;
        _rotationSpeed = Random.Range(_minRotationSpeed, _maxRotationSpeed);
        gameObject.SetActive(true);
        _speed = speed;
        transform.position = position;
    }

    public void OnHide()
    {
        _boxCollider.enabled = false;
        transform.DOScale(Vector3.zero, _hideAnimationDuration).SetLoops(1, LoopType.Yoyo).
            SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }

    private void ChooseSprite()
    {
        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}