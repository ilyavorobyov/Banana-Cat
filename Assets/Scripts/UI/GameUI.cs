using System;
using BananaCatCharacter;
using MissedFruits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
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
        [SerializeField] private Button _gameOverPanelReviveVideoAdButton;
        [SerializeField] private Button _gameOverPanelDoubleScoreVideoAdButton;
        [SerializeField] private Button _soundSwitchMenuButton;
        [SerializeField] private Button _changeBackgroundElementsButton;
        [SerializeField] private Button _infoPanelButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _closeLeaderboardButton;
        [SerializeField] private Button _showAdAtGameSpeedBoostButton;
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
        [SerializeField] private ScoreManager _scoreManager;

        private UIElementsAnimation _uIElementsAnimation;
        private bool _isMobile = false;
        private bool _isReviveVideoAdWatched = false;
        private bool _isDoubleScoreVideoAdWatched = false;

        public static Action<bool> ChangeGameStateEvent;
        public static Action<bool> ChangeMusicEvent;
        public static Action StartGameEvent;
        public static Action HideFallingObjects;
        public static Action GoToMenuEvent;
        public static Action ShowFullScreenAd;
        public static Action ReviveEvent;
        public static Action GameOverEvent;
        public static Action PauseGameEvent;

        public bool IsPaused { get; private set; }

        private void OnEnable()
        {
            YandexGame.GetDataEvent += OnAfterPluginLoad;
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
            AdController.ReviveVideoWatchedCompleteEvent += OnReviveAdVideoWatched;
            AdController.DoubleScoreVideoWatchedCompleteEvent += OnDoubleScoreVideoWatched;
            AdController.AddSpeedAndResumeButtonEvent += delegate { OnChangeStateButtonsClick(true); };
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent -= OnAfterPluginLoad;
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
            AdController.ReviveVideoWatchedCompleteEvent -= OnReviveAdVideoWatched;
            AdController.DoubleScoreVideoWatchedCompleteEvent -= OnDoubleScoreVideoWatched;
            AdController.AddSpeedAndResumeButtonEvent -= delegate { OnChangeStateButtonsClick(true); };
        }

        private void OnAfterPluginLoad()
        {
            _uIElementsAnimation = GetComponent<UIElementsAnimation>();
            _isMobile = YandexGame.EnvironmentData.isMobile;

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
            ChangeGameStateEvent?.Invoke(false);
            ShowRequiredButtons(true);
        }

        public void OnChangeStateButtonsClick(bool isPlayed)
        {
            if (isPlayed)
            {
                Time.timeScale = 1.0f;
                ChangeGameStateEvent?.Invoke(true);
                _uIElementsAnimation.Appear(_pauseButton.gameObject);
                _uIElementsAnimation.Disappear(_pausePanel.gameObject);
                _uIElementsAnimation.Appear(_showAdAtGameSpeedBoostButton.gameObject);
                IsPaused = false;

                if (_isMobile)
                    _touchControl.gameObject.SetActive(true);

                if (_bananaCat.FatLevel > 0)
                    _uIElementsAnimation.Appear(_helpFitText.gameObject);
            }
            else
            {
                Time.timeScale = 0;
                ChangeGameStateEvent?.Invoke(false);
                PauseGameEvent?.Invoke();
                _uIElementsAnimation.Appear(_pausePanel.gameObject);
                _uIElementsAnimation.Disappear(_pauseButton.gameObject);
                _uIElementsAnimation.Disappear(_helpFitText.gameObject);
                _uIElementsAnimation.Disappear(_showAdAtGameSpeedBoostButton.gameObject);
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
            _isDoubleScoreVideoAdWatched = false;

            if (!isRevive)
            {
                StartGameEvent?.Invoke();
                _isReviveVideoAdWatched = false;
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
            _uIElementsAnimation.Disappear(_showAdAtGameSpeedBoostButton.gameObject);
            HideFallingObjects?.Invoke();
            IsPaused = false;
        }

        private void ShowGameOverPanelButtons()
        {
            YandexGame.FullscreenShow();
            Time.timeScale = 0;
            _uIElementsAnimation.Appear(_gameOverPanelRestartButton.gameObject);
            _uIElementsAnimation.Appear(_gameOverPanelMenuButton.gameObject);
            _uIElementsAnimation.Appear(_gameOverPanelReviveVideoAdButton.gameObject);
            _uIElementsAnimation.Appear(_gameOverPanelDoubleScoreVideoAdButton.gameObject);

            if (_isMobile)
                _touchControl.gameObject.SetActive(false);

            if (_isReviveVideoAdWatched)
                _gameOverPanelReviveVideoAdButton.interactable = false;
            else
                _gameOverPanelReviveVideoAdButton.interactable = true;

            if (_isDoubleScoreVideoAdWatched || !_scoreManager.IsAboveZero)
                _gameOverPanelDoubleScoreVideoAdButton.interactable = false;
            else
                _gameOverPanelDoubleScoreVideoAdButton.interactable = true;
        }

        private void OnReviveAdVideoWatched()
        {
            _isReviveVideoAdWatched = true;
            OnStartButtonClick(true);
        }

        private void OnDoubleScoreVideoWatched()
        {
            _isDoubleScoreVideoAdWatched = true;
            _gameOverPanelDoubleScoreVideoAdButton.interactable = false;
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
                _uIElementsAnimation.Disappear(_showAdAtGameSpeedBoostButton.gameObject);

                if (_isMobile)
                {
                    _touchControl.gameObject.SetActive(false);
                    _uIElementsAnimation.Appear(_mobileControlInstructionsText.gameObject);
                }
                else
                {
                    _uIElementsAnimation.Appear(_desktopControlInstructionsText.gameObject);
                }

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
                _uIElementsAnimation.Appear(_showAdAtGameSpeedBoostButton.gameObject);

                if (_isMobile)
                {
                    _touchControl.gameObject.SetActive(true);
                    _uIElementsAnimation.Disappear(_mobileControlInstructionsText.gameObject);
                }
                else
                {
                    _uIElementsAnimation.Disappear(_desktopControlInstructionsText.gameObject);
                }

                if (!CheckAuthorization())
                    _uIElementsAnimation.Disappear(_loginButton.gameObject);

                _uIElementsAnimation.Disappear(_gameOverPanelRestartButton.gameObject);
                _uIElementsAnimation.Disappear(_gameOverPanelMenuButton.gameObject);
                _uIElementsAnimation.Disappear(_gameOverPanelReviveVideoAdButton.gameObject);
                _uIElementsAnimation.Disappear(_gameOverPanelDoubleScoreVideoAdButton.gameObject);
            }
        }
    }
}