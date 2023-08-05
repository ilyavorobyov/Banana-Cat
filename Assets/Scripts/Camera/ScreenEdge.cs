using System;
using UnityEngine;

public class ScreenEdge : MonoBehaviour
{
    [SerializeField] private Spawner[] _spawners;
    [SerializeField] private SideFrame _firstFrame;
    [SerializeField] private SideFrame _secondFrame;

    private int _numberDecimalPlaces = 2;
    private float _reductionNumber = 0.5f;

    private void Awake()
    {
        float cameraWidth = Camera.main.pixelWidth;
        float cameraHeight = Camera.main.pixelHeight;
        float leftEdge = (float)Math.Round((Camera.main.ScreenToWorldPoint(Vector2.zero).x), _numberDecimalPlaces);
        float rightEdge = (float)Math.Round((Camera.main.ScreenToWorldPoint(new Vector2(cameraWidth, 0)).x), _numberDecimalPlaces);
        float upperEdge = (float)Math.Round((Camera.main.ScreenToWorldPoint(new Vector2(0, cameraHeight)).y) + _reductionNumber, _numberDecimalPlaces);
        _firstFrame.transform.position = new Vector3(leftEdge, _firstFrame.transform.position.y, _firstFrame.transform.position.z);
        _secondFrame.transform.position = new Vector3(rightEdge, _secondFrame.transform.position.y, _secondFrame.transform.position.z);
        
        foreach (Spawner spawner in _spawners)
        {
            spawner.Init(leftEdge, rightEdge, upperEdge);
        }
    }
}