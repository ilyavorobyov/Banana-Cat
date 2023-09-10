using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class HealthBarPoint : MonoBehaviour
{
    [SerializeField] private Sprite _inactiveSprite;

    private SpriteRenderer _spriteRenderer;
    private UnityEngine.UI.Image _image;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<UnityEngine.UI.Image>();
    }

    public void BecomeInactive()
    {
        _image.sprite = _inactiveSprite;
    }
}