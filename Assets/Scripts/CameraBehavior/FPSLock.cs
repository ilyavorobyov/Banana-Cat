using UnityEngine;

namespace CameraBehavior
{
    public class FPSLock : MonoBehaviour
    {
        [SerializeField] private int _targetFPS;

        private void Awake()
        {
            Application.targetFrameRate = _targetFPS;
        }
    }
}