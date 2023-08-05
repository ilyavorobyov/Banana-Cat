using UnityEngine;
using UnityEngine.Events;

public class BananaCatCollisionHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _munchSound;

    public event UnityAction FruitTaken;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Fruit fruit))
        {
            _munchSound.PlayDelayed(0);
            FruitTaken.Invoke();
        }
    }
}
