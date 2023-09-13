using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIElementsAnimation))]
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
    [SerializeField] private AudioSource _buttonClickSound;

    private UIElementsAnimation _uIElementsAnimation;

    public static Action<bool> ChangeGameStateEvent;
    public static Action<bool> ChangeMusicEvent;
    public static Action StartGameEvent;
    public static Action HideFallingObjects;
    public static Action TransitionToMenuEvent;

    private void Awake()
    {
        _uIElementsAnimation = GetComponent<UIElementsAnimation>();
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
        _buttonClickSound.PlayDelayed(0);
    }

    private void OnChangeStateButtonsClick(bool isPlayed)
    {
        if(isPlayed)
        {
            Time.timeScale = 1.0f;
            ChangeGameStateEvent?.Invoke(true);
            _uIElementsAnimation.Appear(_pauseButton.gameObject);
            _uIElementsAnimation.Disappear(_pausePanel.gameObject);
        }
        else
        {
            Time.timeScale = 0;
            ChangeGameStateEvent?.Invoke(false);
            _uIElementsAnimation.Appear(_pausePanel.gameObject);
            _uIElementsAnimation.Disappear(_pauseButton.gameObject);
            _uIElementsAnimation.Disappear(_fitText.gameObject);
        }

        _buttonClickSound.PlayDelayed(0);
    }

    private void OnMenuButtonClick()
    {
        TransitionToMenuEvent?.Invoke();
        ChangeMusicEvent?.Invoke(true);
        ChangeGameStateEvent?.Invoke(false);
        ShowRequiredButtons(true);
        HideFallingObjects?.Invoke();
        Time.timeScale = 0;
        _buttonClickSound.PlayDelayed(0);
    }

    private void OnGameOver()
    {
        Time.timeScale = 0;
        HideFallingObjects?.Invoke();
        _uIElementsAnimation.Appear(_gameOverPanel.gameObject);
        _uIElementsAnimation.Disappear(_pauseButton.gameObject);
        _uIElementsAnimation.Disappear(_scoreText.gameObject);
        _uIElementsAnimation.Disappear(_fitText.gameObject);
    }

    private void ShowRequiredButtons(bool onMenu)
    {
        if(onMenu)
        {
            _uIElementsAnimation.Appear(_startButton.gameObject);
            _uIElementsAnimation.Appear(_maxScoreText.gameObject);
            _uIElementsAnimation.Disappear(_pauseButton.gameObject);
            _uIElementsAnimation.Disappear(_resumeButton.gameObject);
            _uIElementsAnimation.Disappear(_menuButton.gameObject);
            _uIElementsAnimation.Disappear(_pausePanel.gameObject);
            _uIElementsAnimation.Disappear(_gameOverPanel.gameObject);
            _uIElementsAnimation.Disappear(_scoreText.gameObject);
            _uIElementsAnimation.Disappear(_healthBar.gameObject);
            _uIElementsAnimation.Disappear(_fitText.gameObject);
        }
        else
        {
            _uIElementsAnimation.Disappear(_startButton.gameObject);
            _uIElementsAnimation.Appear(_pauseButton.gameObject);
            _uIElementsAnimation.Appear(_resumeButton.gameObject);
            _uIElementsAnimation.Appear(_menuButton.gameObject);
            _uIElementsAnimation.Disappear(_pausePanel.gameObject);
            _uIElementsAnimation.Disappear(_gameOverPanel.gameObject);
            _uIElementsAnimation.Appear(_scoreText.gameObject);
            _uIElementsAnimation.Appear(_maxScoreText.gameObject);
            _uIElementsAnimation.Appear(_healthBar.gameObject);
        }
    }
}