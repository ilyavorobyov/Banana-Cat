using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class FallingObject : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Sprite[] _sprites;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseSprite();
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ground ground))
        {
            gameObject.SetActive(false);
        }

        if (collision.TryGetComponent(out BananaCat bananaCat))
        {
            Die(bananaCat);
        }
    }

    public void Init(float speed)
    {

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ChooseSprite()
    {
        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length - 1)];
    }

    public abstract void Die(BananaCat bananaCat);
}
