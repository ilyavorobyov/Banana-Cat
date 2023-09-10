using UnityEngine;

public class MissingFruitMark : MonoBehaviour
{
    private int _lifetime = 1;
    private float _timeFromAppearance;

    private void Update()
    {
        _timeFromAppearance += Time.unscaledDeltaTime;

        if(_timeFromAppearance > _lifetime)
            Destroy(gameObject);
    }
}