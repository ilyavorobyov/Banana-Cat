using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private FallingObject _fallingObject;
    [SerializeField] private int _numberOfObjects;

    private float _minXPosition;
    private float _maxXPosition;
    private float _ySpawnPosition;
    private List<FallingObject> _fallingObjects = new List<FallingObject>();

    private void Awake()
    {
        for (int i = 0; i < _numberOfObjects; i++)
        {
            var fallingObject = Instantiate(_fallingObject, transform.position, Quaternion.identity);
            fallingObject.transform.SetParent(transform, true);
            fallingObject.Hide();
            _fallingObjects.Add(fallingObject);
        }
    }

    private void Start()
    {
        Debug.Log(_ySpawnPosition);
        Debug.Log(_minXPosition);
        Debug.Log(_maxXPosition);

    }

    public void Init(float minXPosition, float maxXPosition, float ySpawnPosition)
    {
        _minXPosition = minXPosition;
        _maxXPosition = maxXPosition;
        _ySpawnPosition = ySpawnPosition;
    }

}