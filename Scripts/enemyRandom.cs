using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Choice
    {
        None,
        Rock,
        Paper,
        Scissors
    }

    public TextMeshProUGUI roundResult;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI enemyScoreText;
    public TextMeshProUGUI RoundNumberTXT;
    public TextMeshProUGUI GameResultText;

    public Image playerHand;
    public Image enemyHand;

    public Sprite rock;
    public Sprite paper;
    public Sprite scissors;

    public GameObject gameOverPanel;

    private int playerScore = 0;
    private int enemyScore = 0;
    private int RoundNumber = 0;

    private Choice playerSelect;

    void Start()
    {
        UpdateUI();
        gameOverPanel.SetActive(false);
    }

    public void PlayerChoice(string choice)
    {
        switch (choice)
        {
            case "Rock":
                playerSelect = Choice.Rock;
                playerHand.sprite = rock;
                break;
            case "Paper":
                playerSelect = Choice.Paper;
                playerHand.sprite = paper;
                break;
            case "Scissors":
                playerSelect = Choice.Scissors;
                playerHand.sprite = scissors;
                break;
        }
        Compare();
    }

    private Choice EnemyPlay()
    {
        int enemySelect = Random.Range(1, 4);
        switch (enemySelect)
        {
            case 1:
                enemyHand.sprite = rock;
                return Choice.Rock;
            case 2:
                enemyHand.sprite = paper;
                return Choice.Paper;
            case 3:
                enemyHand.sprite = scissors;
                return Choice.Scissors;
            default:
                enemyHand.sprite = null;
                return Choice.None;
        }
    }

    private void Compare()
    {
        Choice enemySelect = EnemyPlay();
        if (playerSelect == enemySelect)
        {
            Draw();
        }
        else if ((playerSelect == Choice.Rock && enemySelect == Choice.Scissors) ||
                 (playerSelect == Choice.Paper && enemySelect == Choice.Rock) ||
                 (playerSelect == Choice.Scissors && enemySelect == Choice.Paper))
        {
            Win();
        }
        else
        {
            Lose();
        }
        RoundNumber++;
        UpdateUI();
        CheckGameIsOver();
    }

    private void UpdateUI()
    {
        playerScoreText.text = playerScore.ToString();
        enemyScoreText.text = enemyScore.ToString();
        RoundNumberTXT.text = RoundNumber.ToString();
    }

    private void CheckGameIsOver()
    {
        if (playerScore == 5)
        {
            GameResultText.text = "You win the game!";
            gameOverPanel.SetActive(true);
        }
        else if (enemyScore == 5)
        {
            GameResultText.text = "Game over!";
            gameOverPanel.SetActive(true);
        }
    }

    private void Win()
    {
        roundResult.text = "Win!";
        playerScore++;
    }

    private void Lose()
    {
        roundResult.text = "Lose!";
        enemyScore++;
    }

    private void Draw()
    {
        roundResult.text = "Draw!";
    }
}
