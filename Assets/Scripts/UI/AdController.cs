using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class AdController : MonoBehaviour
    {
        [SerializeField] private Button _reviveVideoAdButton;
        [SerializeField] private Button _doubleScoreVideoAdButton;
        [SerializeField] private Button _addSpeedAndResumeButton;
        [SerializeField] private Button _showAdAtGameSpeedBoostButton;
        [SerializeField] private TMP_Text _rewardAdvErrorText;
        [SerializeField] private AudioSource _reviveSound;

        public static Action ReviveVideoWatchedCompleteEvent;
        public static Action DoubleScoreVideoWatchedCompleteEvent;
        public static Action AddSpeedAndResumeButtonEvent;

        private void OnEnable()
        {
            YandexGame.ErrorVideoEvent += OnErrorShowAd;
            YandexGame.RewardVideoEvent += OnRewardedVideoShow;
            _reviveVideoAdButton.onClick.AddListener(delegate { ShowAd(1); });
            _doubleScoreVideoAdButton.onClick.AddListener(delegate { ShowAd(2); });
            _addSpeedAndResumeButton.onClick.AddListener(delegate { ShowAd(3); });
            _showAdAtGameSpeedBoostButton.onClick.AddListener(delegate { ShowAd(3); });
        }

        private void OnDisable()
        {
            YandexGame.ErrorVideoEvent -= OnErrorShowAd;
            YandexGame.RewardVideoEvent -= OnRewardedVideoShow;
            _reviveVideoAdButton.onClick.RemoveListener(delegate { ShowAd(1); });
            _doubleScoreVideoAdButton.onClick.RemoveListener(delegate { ShowAd(2); });
            _addSpeedAndResumeButton.onClick.RemoveListener(delegate { ShowAd(3); });
            _showAdAtGameSpeedBoostButton.onClick.RemoveListener(delegate { ShowAd(3); });
        }

        private void ShowAd(int id)
        {
            YandexGame.RewVideoShow(id);
        }

        private void OnRewardedVideoShow(int id)
        {
            if (id == 1)
            {
                ReviveVideoWatchedCompleteEvent?.Invoke();
                _reviveSound.PlayDelayed(0);
            }
            else if (id == 2)
            {
                DoubleScoreVideoWatchedCompleteEvent?.Invoke();
            }
            else if (id == 3)
            {
                AddSpeedAndResumeButtonEvent?.Invoke();
            }
        }

        private void OnErrorShowAd()
        {
            _rewardAdvErrorText.gameObject.SetActive(true);
        }
    }
}