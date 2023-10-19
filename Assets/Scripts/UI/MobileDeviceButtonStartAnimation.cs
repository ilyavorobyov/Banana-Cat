using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class MobileDeviceButtonStartAnimation : MonoBehaviour
{
    [SerializeField] private float _startAnimationDuration;

    private Image _buttonImage;
    private Color _defaultColor;

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _defaultColor = _buttonImage.color;
    }

    private void OnEnable()
    {
        GameUI.StartGameEvent += RunStartAnimation;
    }

    private void OnDisable()
    {
        GameUI.StartGameEvent -= RunStartAnimation;
    }

    private void RunStartAnimation()
    {
        _buttonImage.color = _defaultColor;
        _buttonImage.DOFade(0, _startAnimationDuration).SetLoops(2, LoopType.Restart);
    }
}