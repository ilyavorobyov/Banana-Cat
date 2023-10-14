using UnityEngine;

public class BackgroundSelector : MonoBehaviour
{
    [SerializeField] private Background[] _backgrounds;
    [SerializeField] private CloudsParallax[] _clouds;

    private int _currentBackgroundNumber;
    private int _currentCloudsNumber;
    private bool _isBackgroundElementsSelected = false;

    private void Awake()
    {
        ChangeBackground();
    }

    private void ChangeBackground()
    {
        TurnOffBackgroundElement(_backgrounds, _clouds);
        int backgroundNumber = Random.Range(0, _backgrounds.Length);
        int cloudsNumber = Random.Range(0, _clouds.Length);

        if (!_isBackgroundElementsSelected)
        {
            SetBackgroundElements(backgroundNumber, cloudsNumber);
        }
        else
        {
            while (backgroundNumber == _currentBackgroundNumber)
            {
                backgroundNumber = Random.Range(0, _backgrounds.Length);
            }

            while (cloudsNumber == _currentCloudsNumber)
            {
                cloudsNumber = Random.Range(0, _clouds.Length);
            }

            SetBackgroundElements(backgroundNumber, cloudsNumber);
        }

        _isBackgroundElementsSelected = true;
    }

    private void SetBackgroundElements(int backgroundNumber, int cloudsNumber)
    {
        _backgrounds[backgroundNumber].gameObject.SetActive(true);
        _clouds[cloudsNumber].gameObject.SetActive(true);
        _clouds[cloudsNumber].SetSpeed();
        _currentBackgroundNumber = backgroundNumber;
        _currentCloudsNumber = cloudsNumber;
    }

    private void TurnOffBackgroundElement(Background[] backgrounds, CloudsParallax[] clouds)
    {
        foreach (var background in backgrounds)
        { background.gameObject.SetActive(false); }

        foreach (var cloud in clouds)
        { cloud.gameObject.SetActive(false); }
    }
}