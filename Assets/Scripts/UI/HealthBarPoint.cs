using DG.Tweening;
using System.Drawing;
using UnityEngine;
using UnityEngine.XR;
using Color = UnityEngine.Color;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class HealthBarPoint : MonoBehaviour
{
    [SerializeField] private Sprite _inactiveSprite;
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private float _inactiveColorDuration;
    [SerializeField] private float _hideAnimationDuration;

    private Color _initial—olor;

    private UnityEngine.UI.Image _image;

    private void Awake()
    {
        _image = GetComponent<UnityEngine.UI.Image>();
        _initial—olor = _image.color;
    }

    public void BecomeInactive()
    {
        _image.color = _inactiveColor;
        _image.sprite = _inactiveSprite;
        Invoke(nameof(SetInitialColor), _inactiveColorDuration);
    }

    public void HideObject()
    {
        transform.DOScale(Vector3.zero, _hideAnimationDuration).SetLoops(1, LoopType.Yoyo).
        SetUpdate(true).OnComplete(() => Destroy(gameObject));
    }

    private void SetInitialColor()
    {
        _image.color = _initial—olor;
    }
}