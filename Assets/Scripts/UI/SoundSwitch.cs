using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SoundSwitch : MonoBehaviour
{
    [SerializeField] private Sprite _soundOnImage;
    [SerializeField] private Sprite _soundOffImage;

    private Image _soundButtonImage;
    private bool _isPlaying = true;
    private float _maxVolume = 1.0f;
    private float _minVolume = 0;

    private void Awake()
    {
        _soundButtonImage = GetComponent<Image>();
        AudioListener.volume = _maxVolume;
    }

    public void ChangeSoundState()
    {
        _isPlaying = !_isPlaying;

        if (_isPlaying)
        {
            AudioListener.volume = _maxVolume;
            _soundButtonImage.sprite = _soundOnImage;
        }
        else
        {
            AudioListener.volume = _minVolume;
            _soundButtonImage.sprite = _soundOffImage;
        }
    }
}