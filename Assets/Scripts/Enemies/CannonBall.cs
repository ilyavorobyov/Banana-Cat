using BananaCatCharacter;
using MissedFruits;
using UI;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class CannonBall : MonoBehaviour
    {
        private const string HideAnimationName = "Hide";

        [SerializeField] private AudioSource _tapSound;

        private Animator _animator;
        private CircleCollider2D _circleCollider;
        private float _speed = 3.5f;
        private float _hideAnimationDuration = 0.15f;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        private void OnEnable()
        {
            BananaCatCollisionHandler.OpenGameOverPanelEvent += OnHide;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnHide;
            GameUI.HideFallingObjects += OnHide;
            GameUI.GoToMenuEvent += Hide;
        }

        private void OnDisable()
        {
            BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnHide;
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnHide;
            GameUI.HideFallingObjects -= OnHide;
            GameUI.GoToMenuEvent -= Hide;
        }
        private void OnMouseDown()
        {
            _tapSound.PlayDelayed(0);
            OnHide();
        }

        private void Update()
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime, Space.Self);
        }

        public void TurnOnObject()
        {
            _circleCollider.enabled = true;
            gameObject.SetActive(true);
        }

        public void OnHide()
        {
            _circleCollider.enabled = false;
            _animator.SetTrigger(HideAnimationName);
            Invoke(nameof(Hide), _hideAnimationDuration);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}