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
        scoreText.text = "" + score;
        
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score+=amountToIncrease;
        UpdateBar();
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
