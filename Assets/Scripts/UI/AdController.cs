using UnityEngine;
using YG;

public class AdController : MonoBehaviour
{
    private void OnEnable()
    {
        GameUI.ShowFullScreenAd += OnShowFullScreenAd;
    }

    private void OnDisable()
    {
        GameUI.ShowFullScreenAd -= OnShowFullScreenAd;
    }

    private void OnShowFullScreenAd()
    {
        YandexGame.FullscreenShow();
    }
}