using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _menuMusic;

    private void Start()
    {
        _gameMusic.PlayDelayed(0);
    }

    private void OnEnable()
    {
     //   BananaCatMover.StateChangeEvent += OnMusicSwitch;
    }

    private void OnDisable()
    {
     //   BananaCatMover.StateChangeEvent -= OnMusicSwitch;
    }

    private void OnMusicSwitch(bool isCanPlay)
    {
        if(isCanPlay)
        {
            _gameMusic.PlayDelayed(0);
        }
        else
        {
            _gameMusic.Stop();
        }
    }
}