using BananaCatCharacter;
using MissedFruits;
using UI;
using UnityEngine;

namespace Enemies
{
    public class Warning : MonoBehaviour
    {
        private void OnEnable()
        {
            BananaCatCollisionHandler.OpenGameOverPanelEvent += delegate { ShowWarningState(false); };
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent += delegate { ShowWarningState(false); };
            GameUI.HideFallingObjects += delegate { ShowWarningState(false); };
        }

        private void OnDisable()
        {
            BananaCatCollisionHandler.OpenGameOverPanelEvent -= delegate { ShowWarningState(false); };
            MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= delegate { ShowWarningState(false); };
            GameUI.HideFallingObjects -= delegate { ShowWarningState(false); };
        }

        public void ShowWarningState(bool state)
        {
            if (state) gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }
    }
}