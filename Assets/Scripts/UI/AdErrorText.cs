using DG.Tweening;
using UnityEngine;

public class AdErrorText : MonoBehaviour
{
    private float _hideAnimationDuration = 5f;

    private void OnEnable()
    {
        transform.DOScale(Vector3.zero, _hideAnimationDuration).SetLoops(1, LoopType.Yoyo).
        SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }
}