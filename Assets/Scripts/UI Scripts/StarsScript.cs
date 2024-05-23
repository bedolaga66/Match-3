using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StarsScript : MonoBehaviour
{
    public int[] levelScoreGoals;

    public int level;
    public GameObject ButtonAndStarObject;

    private GameObject star1;
    private GameObject star2;
    private GameObject star3;

    private GameData gameData;
    private Board board;
    private ScoreManager scoreManager;
    private World world;

    public int currentHighScore;
    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();
        board = FindObjectOfType<Board>();
        gameData = FindObjectOfType<GameData>();
        scoreManager = FindObjectOfType<ScoreManager>();
        LoadData();
        MakeStarsActive();
    }

    public void LoadData()
    {
        if (gameData != null)
        {
            currentHighScore = gameData.saveData.highScores[level];
        }
    }


    public void MakeStarsActive()
    {
        star1 = ButtonAndStarObject.transform.Find("Star").gameObject;
        star2 = ButtonAndStarObject.transform.Find("Star (1)").gameObject;
        star3 = ButtonAndStarObject.transform.Find("Star (2)").gameObject;
        if (currentHighScore < levelScoreGoals[0] 
            && currentHighScore >= 0)
        {
            star1.SetActive(false);
            star2.SetActive(false);
            star3.SetActive(false);
        }

        if (currentHighScore >= levelScoreGoals[0]
            && currentHighScore < levelScoreGoals[1])
        {
            star1.SetActive(true);
            star2.SetActive(false);
            star3.SetActive(false);
        }

        if (currentHighScore >= levelScoreGoals[1]
            && currentHighScore < levelScoreGoals[2])
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(false);
        }

        if (currentHighScore >= levelScoreGoals[2])
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        }
        currentHighScore = 0;
    }
    
}
