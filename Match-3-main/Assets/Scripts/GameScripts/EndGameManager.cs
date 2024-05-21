using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType
{
    Moves,Time
}

[System.Serializable]
public class EndGameRequiremenets
{
    public GameType gameType;
    public int counterValue;
}

public class EndGameManager : MonoBehaviour
{
    public int currentRaiseValue = 10;
    public ScoreManager scoreManager;
    public GameObject movesLabel;
    //public GameObject timeLabel; //FOR TIME INSTEAD OF MOVES
    public Text counter;
    public EndGameRequiremenets requiremenets;
    public int currentCounterValue;
    private Board board;

    public GameObject youWinPanel;
    public GameObject tryAgainPanel;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        board = FindObjectOfType<Board>();
        SetGameType();
        SetupGame();
    }

    void SetGameType()
    {
        if (board.world != null)
        {
            if (board.level < board.world.levels.Length)
            {
                if (board.world.levels[board.level] != null)
                {
                    requiremenets = board.world.levels[board.level].endGameRequiremenets;
                }
            }
        }
    }

    void SetupGame()
    {
        currentCounterValue = requiremenets.counterValue;
        if(requiremenets.gameType == GameType.Moves)
        {
            movesLabel.SetActive(true);
            //timeLabel.SetActive(false);
        }
        else
        {
            movesLabel.SetActive(false);
            //timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

    public void DecreaseCounterValue()
    {
        if (board.currentState != GameState.pause)
        {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;
            if (currentCounterValue <= 0)
            {
                LoseGame();
            }
            
        }
        
    }

    public void WinGame()
    {
        youWinPanel.SetActive(true);
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindFirstObjectByType<FadePanelController>();
        fade.GameOver();
    }

    public void LoseGame()
    {
        tryAgainPanel.SetActive(true);
        board.currentState = GameState.lose;
        Debug.Log("YOU LOSE");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindFirstObjectByType<FadePanelController>();
        fade.GameOver();
    }
    
    public IEnumerator WinGameAndMovesLeft()
    {
        board.currentState = GameState.wait;
        while(currentCounterValue > 0)
        {
            
            scoreManager.IncreaseScore(currentRaiseValue);
            yield return new WaitForSeconds(0.2f);
            scoreManager.scoreText.text = scoreManager.score.ToString() + "/" + scoreManager.UpdateScoreToGoalAmount(scoreManager.score).ToString();
            currentRaiseValue += 6;
            
            currentCounterValue--;
            yield return new WaitForSeconds(0.2f);
            counter.text = "" + currentCounterValue;
        }
        WinGame();
    }
}
