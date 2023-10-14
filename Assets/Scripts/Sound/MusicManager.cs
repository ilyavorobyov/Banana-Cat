using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _menuMusic;

    private void Start()
    {
        _menuMusic.PlayDelayed(0);
    }

    private void OnEnable()
    {
        GameUI.ChangeMusicEvent += OnMusicSwitch;
    }

    private void OnDisable()
    {
        GameUI.ChangeMusicEvent -= OnMusicSwitch;
    }

    private void OnMusicSwitch(bool onMenu)
    {
        if (!onMenu)
        {
            _menuMusic.Stop();
            _gameMusic.PlayDelayed(0);
        }
        else
        {
            _gameMusic.Stop();
            _menuMusic.PlayDelayed(0);
        }
    }
}