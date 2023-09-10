using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _gameOverPanelMenuButton;
    [SerializeField] private Button _gameOverPanelRestartButton;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _maxScoreText;
    [SerializeField] private TMP_Text _fitText;

    public static Action<bool> ChangeGameStateEvent;
    public static Action<bool> ChangeMusicEvent;
    public static Action StartGameEvent;
    public static Action HideFallingObjects;
    public static Action TransitionToMenuEvent;

    private void Awake()
    {
        ChangeGameStateEvent?.Invoke(false);
        ShowRequiredButtons(true);
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
        _gameOverPanelRestartButton.onClick.AddListener(OnStartButtonClick);
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _gameOverPanelMenuButton.onClick.AddListener(OnMenuButtonClick);
        _pauseButton.onClick.AddListener(delegate { OnChangeStateButtonsClick(false); });
        _resumeButton.onClick.AddListener(delegate { OnChangeStateButtonsClick(true); });
        BananaCatCollisionHandler.GameOverEvent += OnGameOver;
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartButtonClick);
        _gameOverPanelRestartButton.onClick.RemoveListener(OnStartButtonClick);
        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _gameOverPanelMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        _pauseButton.onClick.RemoveListener(delegate { OnChangeStateButtonsClick(false); });
        _resumeButton.onClick.RemoveListener(delegate { OnChangeStateButtonsClick(true); });
        BananaCatCollisionHandler.GameOverEvent -= OnGameOver;
    }

    private void OnStartButtonClick()
    {
        Time.timeScale = 1.0f;
        ChangeGameStateEvent?.Invoke(true);
        ChangeMusicEvent?.Invoke(false);
        ShowRequiredButtons(false);
        StartGameEvent?.Invoke();
    }

    private void OnChangeStateButtonsClick(bool isPlayed)
    {
        if(isPlayed)
        {
            Time.timeScale = 1.0f;
            ChangeGameStateEvent?.Invoke(true);
            _pausePanel.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            ChangeGameStateEvent?.Invoke(false);
            _pausePanel.SetActive(true);
            _pauseButton.gameObject.SetActive(false);
            _fitText.gameObject.SetActive(false);
        }
    }

    private void OnMenuButtonClick()
    {
        TransitionToMenuEvent?.Invoke();
        ChangeMusicEvent?.Invoke(true);
        ChangeGameStateEvent?.Invoke(false);
        ShowRequiredButtons(true);
        HideFallingObjects?.Invoke();
        Time.timeScale = 0;
    }

    private void OnGameOver()
    {
        Time.timeScale = 0;
        HideFallingObjects?.Invoke();
        _gameOverPanel.gameObject.SetActive(true);
        _pauseButton.gameObject.SetActive(false);
        _scoreText.gameObject.SetActive(false);
        _fitText.gameObject.SetActive(false);
    }

    private void ShowRequiredButtons(bool onMenu)
    {
        if(onMenu)
        {
            _startButton.gameObject.SetActive(true);
            _pauseButton.gameObject.SetActive(false);
            _resumeButton.gameObject.SetActive(false);
            _menuButton.gameObject.SetActive(false);
            _pausePanel.SetActive(false);
            _gameOverPanel.SetActive(false);
            _scoreText.gameObject.SetActive(false);
            _maxScoreText.gameObject.SetActive(true);
            _healthBar.SetActive(false);
            _fitText.gameObject.SetActive(false);
        }
        else
        {
            _startButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            _resumeButton.gameObject.SetActive(true);
            _menuButton.gameObject.SetActive(true);
            _pausePanel.SetActive(false);
            _gameOverPanel.SetActive(false);
            _scoreText.gameObject.SetActive(true);
            _maxScoreText.gameObject.SetActive(true);
            _healthBar.SetActive(true);
        }
    }
}