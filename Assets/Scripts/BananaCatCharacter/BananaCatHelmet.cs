using UnityEngine;

namespace BananaCatCharacter
{
    public class BananaCatHelmet : MonoBehaviour
    {
        public void ChangeVisability(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}