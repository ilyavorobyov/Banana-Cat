using DG.Tweening;
using UnityEngine;

public class UIElementsAnimation : MonoBehaviour
{
    [SerializeField] private float _appearanceDuration;
    [SerializeField] private float _disappearanceDuration;


    private void OnValidate()
    {
        float minDuration = 0.2f;
        float maxDuration = 1f;

        if(_appearanceDuration > maxDuration)
            _appearanceDuration = maxDuration;

        if (_appearanceDuration < minDuration)
            _appearanceDuration = minDuration;

        if (_disappearanceDuration > maxDuration)
            _disappearanceDuration = maxDuration;

        if (_disappearanceDuration < minDuration)
            _disappearanceDuration = minDuration;
    }

    public void Appear(GameObject uiElement)
    {
        uiElement.gameObject.transform.localScale = Vector3.zero;
        uiElement.gameObject.SetActive(true);
        uiElement.transform.DOScale(Vector3.one, _appearanceDuration).SetLoops(1, LoopType.Yoyo).
            SetUpdate(true);
    }

    public void Disappear(GameObject uiElement)
    {
        uiElement.transform.DOScale(Vector3.zero, _disappearanceDuration).SetLoops(1, LoopType.Yoyo).
            SetUpdate(true).OnComplete(() => uiElement.gameObject.SetActive(false)); ;
    }
}