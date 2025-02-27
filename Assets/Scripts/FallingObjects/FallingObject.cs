using BananaCatCharacter;
using DG.Tweening;
using MissedFruits;
using UI;
using UnityEngine;

namespace FallingObjects
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class FallingObject : MonoBehaviour
    {
        [SerializeField] protected AudioSource TapSound;
        [SerializeField] private Sprite[] _sprites;

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;
        private float _hideAnimationDuration = 0.5f;
        private float _minRotationSpeed = 1;
        private float _maxRotationSpeed = 2.5f;
        private float _rotationSpeed;
        private float _speed;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            OnChooseSprite();
        }

        private void Update()
        {
            transform.position += Vector3.down * _speed * Time.deltaTime;
            transform.Rotate(0, 0, _rotationSpeed);
        }

        private void OnEnable()
        {
            BananaCatCollisionHandler.OpenGameOverPanelEvent += OnHideObject;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnChooseSprite;
            GameUI.HideFallingObjects += OnHideObject;
        }

        private void OnDisable()
        {
            BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnHideObject;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnChooseSprite;
            GameUI.HideFallingObjects -= OnHideObject;
        }

        public virtual void OnMouseDown() { }

        public void Init(float speed, Vector2 position)
        {
            transform.localScale = Vector3.one;
            _boxCollider.enabled = true;
            _rotationSpeed = Random.Range(_minRotationSpeed, _maxRotationSpeed);
            gameObject.SetActive(true);
            _speed = speed;
            transform.position = position;
        }

        public void OnHideObject()
        {
            _boxCollider.enabled = false;
            transform.DOScale(Vector3.zero, _hideAnimationDuration).SetLoops(1, LoopType.Yoyo).
                SetUpdate(true).OnComplete(() => OnChooseSprite());
        }

        private void OnChooseSprite()
        {
            gameObject.SetActive(false);
            _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
        }

    }
}