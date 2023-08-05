using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStateManager : MonoBehaviour
{
    [SerializeField] private AudioListener _audioListener;
   // [SerializeField] private GameUI _gameUi;

    private void OnApplicationPause(bool pause) // Отправляется всем GameObjects, когда приложение приостанавливается.
    {
        _audioListener.enabled = false;

/*        if (_gameUi.IsGameOn)
        {
            _gameUi.PauseGame();
        }

        // сюда пропиши действия ui при сворачивании игры, то есть чтобы она ставилась на паузу
*/    }

    private void OnApplicationFocus(bool focus)
    {
        _audioListener.enabled = true;
        Debug.Log("test");
    }

}
