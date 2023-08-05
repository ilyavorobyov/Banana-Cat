using UnityEngine;

public class FPSLock : MonoBehaviour
{
    [SerializeField] private int _targetFPS;

    private void Awake()
    {
        Application.targetFrameRate = _targetFPS;
    }
}