using System;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using UnityEngine.XR;

[RequireComponent(typeof(UIElementsAnimation))]
public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _gameOverPanelMenuButton;
    [SerializeField] private Button _gameOverPanelRestartButton;
    [SerializeField] private Button _soundSwitchMenuButton;
    [SerializeField] private Button _changeBackgroundElementsButton;
    [SerializeField] private Button _infoPanelButton;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _maxScoreText;
    [SerializeField] private TMP_Text _fitText;
    [SerializeField] private TMP_Text _desktopControlInstructionsText;
    [SerializeField] private TMP_Text _mobileControlInstructionsText;
    [SerializeField] private AudioSource _buttonClickSound;
    [SerializeField] private TouchControl _touchControl;
    [SerializeField] private float _gameOverScreenDelay;

    private const string FirstRunKey = "FirstRun";

    private UIElementsAnimation _uIElementsAnimation;
    private bool _isMobile = false;

    public static Action<bool> ChangeGameStateEvent;
    public static Action<bool> ChangeMusicEvent;
    public static Action StartGameEvent;
    public static Action HideFallingObjects;
    public static Action TransitionToMenuEvent;
    public static Action ShowFullScreenAd;

    public bool IsPlayed { get; private set; } = false;

    private void Awake()
    {
        _uIElementsAnimation = GetComponent<UIElementsAnimation>();

        if (PlayerPrefs.HasKey(FirstRunKey))
        {
            ChangeGameStateEvent?.Invoke(false);
            ShowRequiredButtons(true);
            _infoPanel.SetActive(false);
        }
        else
        {
            _infoPanel.SetActive(true);
            PlayerPrefs.SetInt(FirstRunKey, 0);
        }
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
        _gameOverPanelRestartButton.onClick.AddListener(OnStartButtonClick);
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _gameOverPanelMenuButton.onClick.AddListener(OnMenuButtonClick);
        _infoPanelButton.onClick.AddListener(OnStartButtonClick);
        _pauseButton.onClick.AddListener(delegate { OnChangeStateButtonsClick(false); });
        _resumeButton.onClick.AddListener(delegate { OnChangeStateButtonsClick(true); });
        BananaCatCollisionHandler.GameOverEvent += OnGameOver;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnGameOver;
        DeviceIdentifier.MobileDeviceDefineEvent += OnEnablingMobileControl;
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartButtonClick);
        _gameOverPanelRestartButton.onClick.RemoveListener(OnStartButtonClick);
        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _gameOverPanelMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        _infoPanelButton.onClick.RemoveListener(OnStartButtonClick);
        _pauseButton.onClick.RemoveListener(delegate { OnChangeStateButtonsClick(false); });
        _resumeButton.onClick.RemoveListener(delegate { OnChangeStateButtonsClick(true); });
        BananaCatCollisionHandler.GameOverEvent -= OnGameOver;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnGameOver;
        DeviceIdentifier.MobileDeviceDefineEvent -= OnEnablingMobileControl;
    }

    private void OnEnablingMobileControl(bool isMobile)
    {
        _isMobile = isMobile;
        ToggleControlInformation();
    }

    public void OnChangeStateButtonsClick(bool isPlayed)
    {
        if (isPlayed)
        {
            Time.timeScale = 1.0f;
            ChangeGameStateEvent?.Invoke(true);
            _uIElementsAnimation.Appear(_pauseButton.gameObject);
            _uIElementsAnimation.Disappear(_pausePanel.gameObject);

            if (_isMobile)
                _uIElementsAnimation.Appear(_touchControl.gameObject);
        }
        else
        {
            Time.timeScale = 0;
            ChangeGameStateEvent?.Invoke(false);
            _uIElementsAnimation.Appear(_pausePanel.gameObject);
            _uIElementsAnimation.Disappear(_pauseButton.gameObject);
            _uIElementsAnimation.Disappear(_fitText.gameObject);

            if (_isMobile)
                _uIElementsAnimation.Disappear(_touchControl.gameObject);
        }

        _buttonClickSound.PlayDelayed(0);
        IsPlayed = isPlayed;
    }

    private void OnStartButtonClick()
    {
        Time.timeScale = 1.0f;
        ChangeGameStateEvent?.Invoke(true);
        ChangeMusicEvent?.Invoke(false);
        ShowRequiredButtons(false);
        StartGameEvent?.Invoke();
        _buttonClickSound.PlayDelayed(0);
        IsPlayed = true;

        if (_infoPanel.gameObject.activeSelf)
            _uIElementsAnimation.Disappear(_infoPanel.gameObject);

        if (_isMobile && _mobileControlInstructionsText.gameObject.activeSelf)
            _uIElementsAnimation.Disappear(_mobileControlInstructionsText.gameObject);
        else if (_desktopControlInstructionsText.gameObject.activeSelf)
            _uIElementsAnimation.Disappear(_desktopControlInstructionsText.gameObject);
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
        IsPlayed = false;
    }

    private void OnGameOver()
    {
        Invoke(nameof(ShowGameOverScreen), _gameOverScreenDelay);
    }

    private void ShowGameOverScreen()
    {
        ShowFullScreenAd?.Invoke();
        Time.timeScale = 0;
        HideFallingObjects?.Invoke();
        _uIElementsAnimation.Appear(_gameOverPanel.gameObject);
        _uIElementsAnimation.Disappear(_pauseButton.gameObject);
        _uIElementsAnimation.Disappear(_scoreText.gameObject);
        _uIElementsAnimation.Disappear(_fitText.gameObject);
        IsPlayed = false;

        if (_isMobile)
            _uIElementsAnimation.Disappear(_touchControl.gameObject);
    }

    private void ShowRequiredButtons(bool onMenu)
    {
        if (onMenu)
        {
            _uIElementsAnimation.Appear(_startButton.gameObject);
            _uIElementsAnimation.Appear(_maxScoreText.gameObject);
            _uIElementsAnimation.Appear(_soundSwitchMenuButton.gameObject);
            _uIElementsAnimation.Appear(_changeBackgroundElementsButton.gameObject);
            _uIElementsAnimation.Disappear(_pauseButton.gameObject);
            _uIElementsAnimation.Disappear(_resumeButton.gameObject);
            _uIElementsAnimation.Disappear(_menuButton.gameObject);
            _uIElementsAnimation.Disappear(_pausePanel.gameObject);
            _uIElementsAnimation.Disappear(_gameOverPanel.gameObject);
            _uIElementsAnimation.Disappear(_scoreText.gameObject);
            _uIElementsAnimation.Disappear(_healthBar.gameObject);
            _uIElementsAnimation.Disappear(_fitText.gameObject);

            if (_isMobile)
                _uIElementsAnimation.Disappear(_touchControl.gameObject);
        }
        else
        {
            _uIElementsAnimation.Disappear(_startButton.gameObject);
            _uIElementsAnimation.Disappear(_changeBackgroundElementsButton.gameObject);
            _uIElementsAnimation.Appear(_pauseButton.gameObject);
            _uIElementsAnimation.Appear(_resumeButton.gameObject);
            _uIElementsAnimation.Appear(_menuButton.gameObject);
            _uIElementsAnimation.Disappear(_pausePanel.gameObject);
            _uIElementsAnimation.Disappear(_gameOverPanel.gameObject);
            _uIElementsAnimation.Disappear(_soundSwitchMenuButton.gameObject);
            _uIElementsAnimation.Appear(_scoreText.gameObject);
            _uIElementsAnimation.Appear(_maxScoreText.gameObject);
            _uIElementsAnimation.Appear(_healthBar.gameObject);

            if (_isMobile)
                _uIElementsAnimation.Appear(_touchControl.gameObject);
        }
    }

    private void ToggleControlInformation()
    {
        if (_isMobile)
            _uIElementsAnimation.Appear(_mobileControlInstructionsText.gameObject);
        else
            _uIElementsAnimation.Appear(_desktopControlInstructionsText.gameObject);
    }
}