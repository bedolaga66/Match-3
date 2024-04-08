using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public Text scoreText;
    public int score;
    public Image scoreBar;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString() + "/" + UpdateScoreToGoalAmount(score).ToString();
        
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score+=amountToIncrease;
        UpdateBar();
    }

    public int UpdateScoreToGoalAmount(int score)
    {
        int i = 0;
        if (score < board.scoreGoals[i])
        {
            return board.scoreGoals[i];
        }
        else if (score >= board.scoreGoals[i] && score < board.scoreGoals[i + 1])
        {
            return board.scoreGoals[i + 1];
        }
        else if (score >= board.scoreGoals[i + 1] && score < board.scoreGoals[i + 2])
        {
            return board.scoreGoals[i + 2];
        }
        else
        {
            return board.scoreGoals[i + 2];
        }
    }

    public void UpdateBar()
    {
        if (board != null && scoreBar != null)
        {
            int len = board.scoreGoals.Length;
            scoreBar.fillAmount = (float)score / (float)board.scoreGoals[len - 1];
        }
    }
}
