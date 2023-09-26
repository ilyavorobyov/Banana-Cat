using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CloudsParallax : MonoBehaviour
{
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private RawImage _image;
    private float _imagePositionX;
    private float _speed;
    private int _reverseDirectionMultiplier = -1;

    private void Start()
    {
        _image = GetComponent<RawImage>();
    }

    private void Update()
    {
        _imagePositionX += _speed * Time.unscaledDeltaTime;

        if (_imagePositionX > 1)
            _imagePositionX = 0;

        _image.uvRect = new Rect(_imagePositionX, 0, _image.uvRect.width, _image.uvRect.height);
    }

    public void SetSpeed()
    {
        _speed = Random.Range(_minSpeed, _maxSpeed);

        if (DefineIsReversedDirection())
            _speed *= _reverseDirectionMultiplier;
    }

    private bool DefineIsReversedDirection()
    {
        int minNumber = 0;
        int maxNumber = 2;
        int number = Random.Range(minNumber, maxNumber);
        return number == minNumber;
    }
}