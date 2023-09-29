using System;
using UnityEngine;
using UnityEngine.Device;

public class ScreenEdge : MonoBehaviour
{
    [SerializeField] private SideFrame _firstFrame;
    [SerializeField] private SideFrame _secondFrame;

    private int _numberDecimalPlaces = 2;
    private float _edgeReducer = 1.3f;

    public static Action<float, float, float> SetSpawnPositions;

    private void OnEnable()
    {
        GameUI.StartGameEvent += GetScreenInfo;
    }

    private void OnDisable()
    {
        GameUI.StartGameEvent -= GetScreenInfo;
    }

    private void GetScreenInfo()
    {
        float cameraWidth = Camera.main.pixelWidth;
        float cameraHeight = Camera.main.pixelHeight;
        float leftEdge = (float)Math.Round((Camera.main.ScreenToWorldPoint(Vector2.zero).x), _numberDecimalPlaces);
        float rightEdge = (float)Math.Round((Camera.main.ScreenToWorldPoint(new Vector2(cameraWidth, 0)).x), _numberDecimalPlaces);
        float upperEdge = (float)Math.Round((Camera.main.ScreenToWorldPoint(new Vector2(0, cameraHeight)).y) + _edgeReducer, _numberDecimalPlaces);
        _firstFrame.transform.position = new Vector3(leftEdge, _firstFrame.transform.position.y, _firstFrame.transform.position.z);
        _secondFrame.transform.position = new Vector3(rightEdge, _secondFrame.transform.position.y, _secondFrame.transform.position.z);
        SetSpawnPositions?.Invoke(leftEdge + _edgeReducer, rightEdge - _edgeReducer, upperEdge);
    }
}