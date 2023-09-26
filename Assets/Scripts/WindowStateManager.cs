using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStateManager : MonoBehaviour
{
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private GameUI _gameUi;

    private void OnApplicationPause(bool pause)
    {
        _audioListener.enabled = false;

        if (_gameUi.IsPlayed)
        {
            _gameUi.OnChangeStateButtonsClick(false);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        _audioListener.enabled = true;
    }
}