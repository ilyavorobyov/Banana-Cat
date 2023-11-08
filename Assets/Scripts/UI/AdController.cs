using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class AdController : MonoBehaviour
{
    [SerializeField] private Button _reviveVideoAdButton;
    [SerializeField] private Button _doubleScoreVideoAdButton;
    [SerializeField] private TMP_Text _rewardAdvErrorText;
    [SerializeField] private AudioSource _reviveSound;

    public static Action ReviveVideoWatchedCompleteEvent;
    public static Action DoubleScoreVideoWatchedCompleteEvent;

    private void OnEnable()
    {
        YandexGame.ErrorVideoEvent += OnErrorShowAd;
        YandexGame.RewardVideoEvent += OnRewardedVideoShow;
        _reviveVideoAdButton.onClick.AddListener(delegate { ShowAd(1); });
        _doubleScoreVideoAdButton.onClick.AddListener(delegate { ShowAd(2); });
    }

    private void OnDisable()
    {
        YandexGame.ErrorVideoEvent -= OnErrorShowAd;
        YandexGame.RewardVideoEvent -= OnRewardedVideoShow;
        _reviveVideoAdButton.onClick.RemoveListener(delegate { ShowAd(1); });
        _doubleScoreVideoAdButton.onClick.RemoveListener(delegate { ShowAd(2); });
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
            Debug.Log("double result");
        }
    }

    private void OnErrorShowAd()
    {
        _rewardAdvErrorText.gameObject.SetActive(true);
    }
}