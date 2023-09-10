using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _maxScoreText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverPanelText;
    [SerializeField] private Color _newRecordColor;
    [SerializeField] private Color _startingScoreTextColor;
    [SerializeField] private int _scoreDivisor;

    private const string MaxScoreKey = "MaxScore";

    private int _maxScore;
    private int _score;
    private int _minScoreDivisor = 5;

    public static Action AddDifficultyEvent;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(MaxScoreKey))
            _maxScore = PlayerPrefs.GetInt(MaxScoreKey);
        else
            _maxScore = 0;

        if(_maxScore != 0)
            _maxScoreText.text = _maxScore.ToString();
    }

    private void OnEnable()
    {
        BananaCatCollisionHandler.FruitTakenEvent += OnFruitTaken;
        BananaCatCollisionHandler.GameOverEvent += OnGameOverScoreCompare;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.FruitTakenEvent -= OnFruitTaken;
        BananaCatCollisionHandler.GameOverEvent -= OnGameOverScoreCompare;
    }

    private void OnValidate()
    {
        if (_scoreDivisor < _minScoreDivisor)
            _scoreDivisor = _minScoreDivisor;
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
            PlayerPrefs.SetInt(MaxScoreKey, _score);
            _gameOverPanelText.text = "Новый рекорд: " + _score;
            _maxScore = _score;
            _maxScoreText.text = _maxScore.ToString();
        }
        else
            _gameOverPanelText.text = "Результат: " + _score;

        _score = 0;
        _scoreText.text = _score.ToString();
        _scoreText.color = _startingScoreTextColor;
    }
}