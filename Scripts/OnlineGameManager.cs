using FishNet.Object;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineGameManager : NetworkBehaviour
{
    NetworkControl networkControl;
    public enum Choice { None, Rock, Paper, Scissors }

    public TextMeshProUGUI roundResult;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI enemyScoreText;
    public TextMeshProUGUI RoundNumberTXT;
    public TextMeshProUGUI GameResultText;

    public Image blueHand;
    public Image redHand;

    public Sprite check;
    public Sprite noneSprite;
    public Sprite rock;
    public Sprite paper;
    public Sprite scissors;

    public GameObject gameOverPanel;

    private int blueScore = 0;
    private int redScore = 0;
    private int RoundNumber = 1;

    private Choice blueChoice = Choice.None;
    private Choice redChoice = Choice.None;

    public void RestartGame()
    {
        blueScore = 0;
        redScore = 0;
        RoundNumber = 1;

        blueChoice = Choice.None;
        redChoice = Choice.None;

        UpdateUI();

        GameResultText.text = "";
        gameOverPanel.SetActive(false);

        blueHand.sprite = noneSprite;
        redHand.sprite = noneSprite;
    }

    private void Start()
    {
        networkControl = FindAnyObjectByType<NetworkControl>();
    }

    public void CallSumbit(Choice choice)
    {
        if (networkControl.myPlayer == NetworkControl.PlayerType.Host)
        {
            SubmitChoice(choice, true);
        }
        else if (networkControl.myPlayer == NetworkControl.PlayerType.Client)
        {
            SubmitChoice(choice, false);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void SubmitChoice(Choice choice, bool isBlue)
    {
        if (isBlue)
        {
            blueChoice = choice;
        }
        else
        {
            redChoice = choice;
        }
        ShowStatus(isBlue);


        if (blueChoice != Choice.None && redChoice != Choice.None)
        {
            ShowChoice(blueChoice, true);
            ShowChoice(redChoice, false);
            //CompareChoices();
            CompareChoicesServer();
        }
    }

    [ObserversRpc]
    private void ShowChoice(Choice choice, bool isBlue)
    {
        if (isBlue)
            blueHand.sprite = GetSprite(choice);
        else
            redHand.sprite = GetSprite(choice);
    }
    [ObserversRpc]
    private void ShowStatus( bool isBlue)
    {
        if (isBlue)
            blueHand.sprite = check;
        else
            redHand.sprite = check;
    }


    [ObserversRpc]
    private void CompareChoices()
    {
        if (blueChoice != Choice.None && blueChoice == redChoice)
        {
            roundResult.text = "Draw!";
        }
        else if ((blueChoice == Choice.Rock && redChoice == Choice.Scissors) ||
                 (blueChoice == Choice.Paper && redChoice == Choice.Rock) ||
                 (blueChoice == Choice.Scissors && redChoice == Choice.Paper))
        {
            blueScore++;
            roundResult.text = "Blue Wins!";
        }
        else
        {
            redScore++;
            roundResult.text = "Red Wins!";
        }

        RoundNumber++;
        UpdateUI();
        CheckGameIsOver();

        blueChoice = Choice.None;
        redChoice = Choice.None;


        //blueHand.sprite = noneSprite;
        //redHand.sprite = noneSprite;
    }

    [ServerRpc(RequireOwnership = false)]
    private void CompareChoicesServer()
    {
        string result;
        if (blueChoice == redChoice)
        {
            result = "Draw!";
        }
        else if ((blueChoice == Choice.Rock && redChoice == Choice.Scissors) ||
                 (blueChoice == Choice.Paper && redChoice == Choice.Rock) ||
                 (blueChoice == Choice.Scissors && redChoice == Choice.Paper))
        {
            blueScore++;
            result = "Blue Wins!";
        }
        else
        {
            redScore++;
            result = "Red Wins!";
        }
        ShowResult(result, blueScore, redScore);
        ResetChoices();
    }
 

    [ObserversRpc]
    private void ShowResult(string result, int bScore, int rScore)
    {
        blueScore = bScore;
        redScore = rScore;
        RoundNumber++;
        roundResult.text = result;
        UpdateUI();
        CheckGameIsOver();
    }

    private void ResetChoices()
    {
        blueChoice = Choice.None;
        redChoice = Choice.None;
    }
    private Sprite GetSprite(Choice choice)
    {
        return choice switch
        {
            Choice.Rock => rock,
            Choice.Paper => paper,
            Choice.Scissors => scissors,
            _ => null,
        };
    }

    private void UpdateUI()
    {
        playerScoreText.text = blueScore.ToString();
        enemyScoreText.text = redScore.ToString();
        RoundNumberTXT.text = RoundNumber.ToString();
    }

    private void CheckGameIsOver()
    {
        if (blueScore == 5)
        {
            if(networkControl.myPlayer == NetworkControl.PlayerType.Host)
            {
                GameResultText.text = "You won the game!";
                gameOverPanel.SetActive(true);
            }
            else
            {
                GameResultText.text = "You lost the game.";
                gameOverPanel.SetActive(true);

            }

        }
        else if (redScore == 5)
        {
            if (networkControl.myPlayer == NetworkControl.PlayerType.Client)
            {
                GameResultText.text = "You won the game!";
                gameOverPanel.SetActive(true);
            }
            else
            {
                GameResultText.text = "You lost the game!";
                gameOverPanel.SetActive(true);
            }
        }
    }

    //Choose Functions
    public void ChooseRock() => CallSumbit(Choice.Rock);

    public void ChoosePaper() => CallSumbit(Choice.Paper);
    public void ChooseScissors() => CallSumbit(Choice.Scissors);
}
