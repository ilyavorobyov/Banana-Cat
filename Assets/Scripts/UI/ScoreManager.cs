using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _maxScoreText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _newRecordPanelText;
    [SerializeField] private TMP_Text _resultPanelText;
    [SerializeField] private TMP_Text _scorePanelText;
    [SerializeField] private Image _maxScoreImage;
    [SerializeField] private Color _newRecordColor;
    [SerializeField] private Color _startingScoreTextColor;
    [SerializeField] private int _scoreDivisor;

    private const string MaxScoreKey = "MaxScore";

    private int _maxScore;
    private int _score;
    private int _minScoreDivisor = 5;

    public static Action AddDifficultyEvent;

    private void OnEnable()
    {
        BananaCatCollisionHandler.FruitTakenEvent += OnFruitTaken;
        BananaCatCollisionHandler.OpenGameOverPanelEvent += OnGameOverScoreCompare;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent += OnGameOverScoreCompare;
        GameUI.GameOverEvent += ResetScoreValues;
        YandexGame.GetDataEvent += OnLoadScore;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.FruitTakenEvent -= OnFruitTaken;
        BananaCatCollisionHandler.OpenGameOverPanelEvent -= OnGameOverScoreCompare;
        MissedFruitsCounter.MaxFruitsNumberDroppedEvent -= OnGameOverScoreCompare;
        GameUI.GameOverEvent -= ResetScoreValues;
        YandexGame.GetDataEvent -= OnLoadScore;
    }

    private void OnValidate()
    {
        if (_scoreDivisor < _minScoreDivisor)
            _scoreDivisor = _minScoreDivisor;
    }

    private void OnLoadScore()
    {
        _maxScore = YandexGame.savesData.MaxScore;

        if(_maxScore > 0)
        {
            _maxScoreImage.gameObject.SetActive(true);
            _maxScoreText.gameObject.SetActive(true);
            _maxScoreText.text = _maxScore.ToString();
        }
    }

    private void OnFruitTaken()
    {
        _score++;
        _scoreText.text = _score.ToString();

        if (_score == _maxScore && _maxScore != 0)
            _scoreText.color = _newRecordColor;

        if (_score % _scoreDivisor == 0)
            AddDifficultyEvent?.Invoke();
    }

    private void OnGameOverScoreCompare()
    {
        if(_score > _maxScore)
        {
            _scorePanelText.text = _score.ToString();
            _newRecordPanelText.gameObject.SetActive(true);
            _resultPanelText.gameObject.SetActive(false);
            _maxScore = _score;
            _maxScoreText.text = _maxScore.ToString();
            _maxScoreImage.gameObject.SetActive(true);
            _maxScoreText.gameObject.SetActive(true);
            YandexGame.savesData.MaxScore = _score;
            YandexGame.SaveProgress();

        }
        else
        {
            _scorePanelText.text = _score.ToString();
            _newRecordPanelText.gameObject.SetActive(false);
            _resultPanelText.gameObject.SetActive(true);
        }
    }

    private void ResetScoreValues()
    {
        _score = 0;
        _scoreText.text = _score.ToString();
        _scoreText.color = _startingScoreTextColor;
    }
}