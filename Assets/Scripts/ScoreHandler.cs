using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler: MonoBehaviour
{
    [SerializeField] private PlayerScore _playerScore;
    [SerializeField] private Text _scoreText;

    private void Start()
    {
        _playerScore.score = 0;
    }

    public void AddPoint()
    {
        _playerScore.score++;
    }

    public void UpdateScoreText()
    {
        _scoreText.text = _playerScore.score.ToString();
    }
}