using Environment;
using UnityEngine;

namespace Enemies
{
    public class BatEnemyCollisionHandler : MonoBehaviour
    {
        private bool _isCollided = false;

        private void OnEnable()
        {
            _isCollided = false;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out SideFrame sideFrame))
            {
                if (_isCollided)
                {
                    gameObject.SetActive(false);
                }

                _isCollided = true;
            }
        }
    }
}