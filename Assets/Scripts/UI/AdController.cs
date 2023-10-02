using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class AdController : MonoBehaviour
{
    [SerializeField] private Button _rewardedVideoButton;
    [SerializeField] private TMP_Text _rewardAdvErrorText;
    [SerializeField] private AudioSource _reviveSound;

    public static Action ReviveVideoWatchedCompleteEvent;

    private void OnEnable()
    {
        YandexGame.ErrorVideoEvent += OnErrorShowAd; 
        YandexGame.CloseVideoEvent += OnCloseAd;
        GameUI.ShowFullScreenAd += delegate { ShowAd(false); };
        _rewardedVideoButton.onClick.AddListener(delegate { ShowAd(true); });
    }

    private void OnDisable()
    {
        YandexGame.ErrorVideoEvent -= OnErrorShowAd;
        YandexGame.CloseVideoEvent -= OnCloseAd;
        GameUI.ShowFullScreenAd -= delegate { ShowAd(false); };
        _rewardedVideoButton.onClick.RemoveListener(delegate { ShowAd(true); });
    }

    private void ShowAd(bool isRewarded)
    {
        if (isRewarded)
            YandexGame.RewVideoShow(0);
        else
            YandexGame.FullscreenShow();
    }

    private void OnErrorShowAd()
    {
        _rewardAdvErrorText.gameObject.SetActive(true);
    }

    private void OnCloseAd()
    {
        ReviveVideoWatchedCompleteEvent?.Invoke();
        _reviveSound.PlayDelayed(0);
    }
}