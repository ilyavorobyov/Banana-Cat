using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStateManager : MonoBehaviour
{
    [SerializeField] private AudioListener _audioListener;
   // [SerializeField] private GameUI _gameUi;

    private void OnApplicationPause(bool pause) // ������������ ���� GameObjects, ����� ���������� ������������������.
    {
        _audioListener.enabled = false;

/*        if (_gameUi.IsGameOn)
        {
            _gameUi.PauseGame();
        }

        // ���� ������� �������� ui ��� ������������ ����, �� ���� ����� ��� ��������� �� �����
*/    }

    private void OnApplicationFocus(bool focus)
    {
        _audioListener.enabled = true;
        Debug.Log("test");
    }

}
