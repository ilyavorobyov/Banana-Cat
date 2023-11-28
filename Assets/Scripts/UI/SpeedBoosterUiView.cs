using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoosterUiView : MonoBehaviour
{
    [SerializeField] private UIElementsAnimation _uIElementsAnimation;
    [SerializeField] private Image _speedBoosterIcon;
    [SerializeField] private float _speedBoostDuration;
    [SerializeField] private GameObject _speedAddInfoText;
    [SerializeField] private float _textDisplayDuration;
    [SerializeField] private AudioSource _addSpeedSound;
    [SerializeField] private AudioSource _removeAddSpeedSound;

    private Coroutine _fillingBoosterIcon;

    public static Action StopAddSpeedEvent;

    private void OnEnable()
    {
        BananaCatCollisionHandler.TookSpeedBoosterEvent += OnTookSpeedBooster;
        BananaCatCollisionHandler.OpenGameOverPanelEvent += OnHideAll;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnHideAll;
        GameUI.GoToMenuEvent += OnHideAll;
        GameUI.PauseGameEvent += HideSpeedAddText;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.TookSpeedBoosterEvent -= OnTookSpeedBooster;
        BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnHideAll;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnHideAll;
        GameUI.GoToMenuEvent -= OnHideAll;
        GameUI.PauseGameEvent -= HideSpeedAddText;
    }

    private void BeginFillBoosterIcon()
    {
        StopFillBoosterIcon();
        _fillingBoosterIcon = StartCoroutine(FillingBoosterIcon());
    }

    private void StopFillBoosterIcon()
    {
        if (_fillingBoosterIcon != null)
            StopCoroutine(_fillingBoosterIcon);
    }

    private void OnTookSpeedBooster()
    {
        _uIElementsAnimation.Appear(_speedBoosterIcon.gameObject);
        _addSpeedSound.PlayDelayed(0);
        ShowSpeedAddText();
        BeginFillBoosterIcon();
    }

    private void ShowSpeedAddText()
    {
        _uIElementsAnimation.Appear(_speedAddInfoText.gameObject);
        _speedAddInfoText.gameObject.SetActive(true);
        Invoke(nameof(HideSpeedAddText), _textDisplayDuration);
    }

    private void HideSpeedAddText()
    {
        _uIElementsAnimation.Disappear(_speedAddInfoText.gameObject);
    }

    private void StopAddSpeed()
    {
        _removeAddSpeedSound.PlayDelayed(0);
        StopAddSpeedEvent?.Invoke();
    }

    private void OnHideAll()
    {
        StopFillBoosterIcon();
        HideSpeedAddText();
        _uIElementsAnimation.Disappear(_speedBoosterIcon.gameObject);
    }

    private IEnumerator FillingBoosterIcon()
    {
        float elapsedTime = 0;
        float nextValue;

        while (elapsedTime < _speedBoostDuration)
        {
            nextValue = Mathf.Lerp(1, 0, elapsedTime / _speedBoostDuration);
            _speedBoosterIcon.fillAmount = nextValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StopAddSpeed();
        _uIElementsAnimation.Disappear(_speedBoosterIcon.gameObject);
    }
}
