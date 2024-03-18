using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshPro scoreText;
    private int score = 0;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        score = gameManager.score;
        // Assure que le score initial est affiché correctement
        UpdateScoreText();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        gameManager.score = score;
        UpdateScoreText();
    }

    public void DecreaseScore(int amount)
    {
        score -= amount;
        UpdateScoreText();
    }

    private void Update()
    {
        scoreText.text = "Score: " + gameManager.score.ToString();
    }
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
