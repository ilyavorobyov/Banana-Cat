using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(UIElementsAnimation))]
public class GameUI : MonoBehaviour
{
    [SerializeField] private BananaCat _bananaCat;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _gameOverPanelMenuButton;
    [SerializeField] private Button _gameOverPanelRestartButton;
    [SerializeField] private Button _gameOverRewardedVideoButton;
    [SerializeField] private Button _soundSwitchMenuButton;
    [SerializeField] private Button _changeBackgroundElementsButton;
    [SerializeField] private Button _infoPanelButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _closeLeaderboardButton;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _leaderboard;
    [SerializeField] private LeaderboardYG _leaderboardYG;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _helpFitText;
    [SerializeField] private TMP_Text _desktopControlInstructionsText;
    [SerializeField] private TMP_Text _mobileControlInstructionsText;
    [SerializeField] private AudioSource _buttonClickSound;
    [SerializeField] private TouchControl _touchControl;
    [SerializeField] private float _gameOverScreenDelay;

    private UIElementsAnimation _uIElementsAnimation;
    private bool _isMobile = false;
    private bool _isAdVideoWatched = false;

    public static Action<bool> ChangeGameStateEvent;
    public static Action<bool> ChangeMusicEvent;
    public static Action StartGameEvent;
    public static Action HideFallingObjects;
    public static Action GoToMenuEvent;
    public static Action ShowFullScreenAd;
    public static Action ReviveEvent;
    public static Action GameOverEvent;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        _uIElementsAnimation = GetComponent<UIElementsAnimation>();
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(delegate { OnStartButtonClick(false); });
        _gameOverPanelRestartButton.onClick.AddListener(delegate { OnStartButtonClick(false); });
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _gameOverPanelMenuButton.onClick.AddListener(OnMenuButtonClick);
        _infoPanelButton.onClick.AddListener(OnInfoPanelButtonClick);
        _pauseButton.onClick.AddListener(delegate { OnChangeStateButtonsClick(false); });
        _resumeButton.onClick.AddListener(delegate { OnChangeStateButtonsClick(true); });
        _leaderboardButton.onClick.AddListener(OnShowLeaderboard);
        _closeLeaderboardButton.onClick.AddListener(OnCloseLeaderboard);
        BananaCatCollisionHandler.OpenGameOverPanelEvent += OnLose;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnLose;
        UserDataReader.MobileDeviceDefineEvent += OnEnablingMobileControl;
        AdController.ReviveVideoWatchedCompleteEvent += OnReviveAdVideoWatched;
        YandexGame.GetDataEvent += OnAfterPluginLoad;
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(delegate { OnStartButtonClick(false); });
        _gameOverPanelRestartButton.onClick.RemoveListener(delegate { OnStartButtonClick(false); });
        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _gameOverPanelMenuButton.onClick.RemoveListener(OnMenuButtonClick);
        _infoPanelButton.onClick.RemoveListener(OnInfoPanelButtonClick);
        _leaderboardButton.onClick.RemoveListener(OnShowLeaderboard);
        _pauseButton.onClick.RemoveListener(delegate { OnChangeStateButtonsClick(false); });
        _resumeButton.onClick.RemoveListener(delegate { OnChangeStateButtonsClick(true); });
        _closeLeaderboardButton.onClick.RemoveListener(OnCloseLeaderboard);
        BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnLose;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnLose;
        UserDataReader.MobileDeviceDefineEvent -= OnEnablingMobileControl;
        AdController.ReviveVideoWatchedCompleteEvent -= OnReviveAdVideoWatched;
        YandexGame.GetDataEvent -= OnAfterPluginLoad;
    }

    private void OnAfterPluginLoad()
    {
        if (YandexGame.savesData.IsAlreadyOpened)
        {
            _infoPanel.SetActive(false);
            ChangeGameStateEvent?.Invoke(false);
            ShowRequiredButtons(true);
            _infoPanel.SetActive(false);
        }
        else
        {
            _infoPanel.SetActive(true);
            YandexGame.savesData.IsAlreadyOpened = true;
            YandexGame.SaveProgress();
        }
    }

    private bool CheckAuthorization()
    {
        if (YandexGame.auth)
            return true;
        else
            return false;
    }

    private void OnShowLeaderboard()
    {
        _leaderboardYG.UpdateLB();
        _uIElementsAnimation.Appear(_leaderboard);
    }

    private void OnCloseLeaderboard()
    {
        _uIElementsAnimation.Disappear(_leaderboard);
    }

    private void OnInfoPanelButtonClick()
    {
        _uIElementsAnimation.Disappear(_infoPanel.gameObject);
        ShowRequiredButtons(true);
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
            IsPaused = false;

            if (_isMobile)
                _touchControl.gameObject.SetActive(true);

            if (_bananaCat.FatLevel > 0)
            {
                _uIElementsAnimation.Appear(_helpFitText.gameObject);
            }
        }
        else
        {
            Time.timeScale = 0;
            ChangeGameStateEvent?.Invoke(false);
            _uIElementsAnimation.Appear(_pausePanel.gameObject);
            _uIElementsAnimation.Disappear(_pauseButton.gameObject);
            _uIElementsAnimation.Disappear(_helpFitText.gameObject);
            IsPaused = true;

            if (_isMobile)
                _touchControl.gameObject.SetActive(false);
        }

        _buttonClickSound.PlayDelayed(0);
    }

    private void OnStartButtonClick(bool isRevive)
    {
        Time.timeScale = 1.0f;
        ChangeGameStateEvent?.Invoke(true);
        ChangeMusicEvent?.Invoke(false);
        ShowRequiredButtons(false);

        if (!isRevive)
        {
            StartGameEvent?.Invoke();
            _isAdVideoWatched = false;
            GameOverEvent?.Invoke();
        }
        else
            ReviveEvent?.Invoke();

        _buttonClickSound.PlayDelayed(0);

        if (_infoPanel.gameObject.activeSelf)
            _uIElementsAnimation.Disappear(_infoPanel.gameObject);

        if (_isMobile && _mobileControlInstructionsText.gameObject.activeSelf)
            _uIElementsAnimation.Disappear(_mobileControlInstructionsText.gameObject);
        else if (_desktopControlInstructionsText.gameObject.activeSelf)
            _uIElementsAnimation.Disappear(_desktopControlInstructionsText.gameObject);
    }

    private void OnMenuButtonClick()
    {
        GoToMenuEvent?.Invoke();
        ChangeMusicEvent?.Invoke(true);
        ChangeGameStateEvent?.Invoke(false);
        GameOverEvent?.Invoke();
        ShowRequiredButtons(true);
        HideFallingObjects?.Invoke();
        Time.timeScale = 0;
        _buttonClickSound.PlayDelayed(0);
        IsPaused = false;
    }

    private void OnLose()
    {
        Invoke(nameof(ShowGameOverPanelButtons), _gameOverScreenDelay);
        _uIElementsAnimation.Appear(_gameOverPanel.gameObject);
        _uIElementsAnimation.Disappear(_scoreText.gameObject);
        _uIElementsAnimation.Disappear(_helpFitText.gameObject);
        _uIElementsAnimation.Disappear(_pauseButton.gameObject);
        HideFallingObjects?.Invoke();
        IsPaused = false;
    }

    private void ShowGameOverPanelButtons()
    {
        ShowFullScreenAd?.Invoke();
        Time.timeScale = 0;
        _uIElementsAnimation.Appear(_gameOverPanelRestartButton.gameObject);
        _uIElementsAnimation.Appear(_gameOverPanelMenuButton.gameObject);
        _uIElementsAnimation.Appear(_gameOverRewardedVideoButton.gameObject);

        if (_isMobile)
            _touchControl.gameObject.SetActive(false);

        if (_isAdVideoWatched)
            _gameOverRewardedVideoButton.interactable = false;
        else
            _gameOverRewardedVideoButton.interactable = true;
    }

    private void OnReviveAdVideoWatched()
    {
        _isAdVideoWatched = true;
        OnStartButtonClick(true);
    }

    private void ShowRequiredButtons(bool onMenu)
    {
        if (onMenu)
        {
            _uIElementsAnimation.Appear(_startButton.gameObject);
            _uIElementsAnimation.Appear(_soundSwitchMenuButton.gameObject);
            _uIElementsAnimation.Appear(_changeBackgroundElementsButton.gameObject);
            _uIElementsAnimation.Appear(_leaderboardButton.gameObject);
            _uIElementsAnimation.Disappear(_pauseButton.gameObject);
            _uIElementsAnimation.Disappear(_resumeButton.gameObject);
            _uIElementsAnimation.Disappear(_menuButton.gameObject);
            _uIElementsAnimation.Disappear(_pausePanel.gameObject);
            _uIElementsAnimation.Disappear(_gameOverPanel.gameObject);
            _uIElementsAnimation.Disappear(_scoreText.gameObject);
            _uIElementsAnimation.Disappear(_healthBar.gameObject);
            _uIElementsAnimation.Disappear(_helpFitText.gameObject);

            if (_isMobile)
                _touchControl.gameObject.SetActive(false);

            if (!CheckAuthorization())
                _uIElementsAnimation.Appear(_loginButton.gameObject);
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
            _uIElementsAnimation.Disappear(_leaderboardButton.gameObject);
            _uIElementsAnimation.Appear(_scoreText.gameObject);
            _uIElementsAnimation.Appear(_healthBar.gameObject);

            if (_isMobile)
                _touchControl.gameObject.SetActive(true);

            if (!CheckAuthorization())
                _uIElementsAnimation.Disappear(_loginButton.gameObject);

            _uIElementsAnimation.Disappear(_gameOverPanelRestartButton.gameObject);
            _uIElementsAnimation.Disappear(_gameOverPanelMenuButton.gameObject);
            _uIElementsAnimation.Disappear(_gameOverRewardedVideoButton.gameObject);
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