using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.Example;

public class AdvController : MonoBehaviour
{
    [SerializeField] private Button _rewardedVideoButton;
    [SerializeField] private TMP_Text _rewardAdInfoText;
    [SerializeField] private AudioSource _reviveSound;
    [SerializeField] private GameUI _gameUi;

    public static Action ReviveVideoWatchedCompleteEvent;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnRewarded;
        YandexGame.ErrorVideoEvent += OnErrorVideoAd;
        YandexGame.CloseVideoEvent += OnCloseVideoAd;
        _rewardedVideoButton.onClick.AddListener(OnOpenRewardedVideo);
        GameUI.ShowFullScreenAd += OnShowFullScreenAd;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnRewarded;
        YandexGame.ErrorVideoEvent -= OnErrorVideoAd;
        YandexGame.CloseVideoEvent -= OnCloseVideoAd;
        _rewardedVideoButton.onClick.RemoveListener(OnOpenRewardedVideo);
        GameUI.ShowFullScreenAd -= OnShowFullScreenAd;
    }

    private void OnShowFullScreenAd()
    {
        YandexGame.FullscreenShow();
    }

    private void OnRewarded(int id)
    {
        if(id == 0)
        {
            ReviveVideoWatchedCompleteEvent?.Invoke();
            _rewardAdInfoText.gameObject.SetActive(true);
        }
    }

    private void OnOpenRewardedVideo()
    {
        YandexGame.RewVideoShow(0);
    }

    private void OnErrorVideoAd()
    {
        _rewardAdInfoText.gameObject.SetActive(true);
    }

    private void OnCloseVideoAd()
    {
        _reviveSound.PlayDelayed(0);
    }
}