using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
}

public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    private EndGameManager endGame;
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        endGame = FindObjectOfType<EndGameManager>();
        GetGoals();
        SetupIntroGoals();
        ResetGoalsOnLevel();
    }

    void GetGoals()
    {
        if( board != null)
        {
            if(board.world != null)
            {
                if (board.level < board.world.levels.Length)
                {
                    if (board.world.levels[board.level] != null)
                    {
                        levelGoals = board.world.levels[board.level].levelGoals;
                    }
                }
            }
        }
    }

    void SetupIntroGoals()
    {
        for(int i =0; i < levelGoals.Length; i++)
        {
            //Create new Goal Panel at the goalIntroParent position
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position,
                Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform);
            //Set the imaage and text of the goal
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;

            //Create a new goal panel at the goalGameParent pos
            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position,
                Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform);
            //Set the imaage and text of the goal
            panel = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
        }
    }

    public void UpdateGoals()
    {
        int goalsCompleted = 0;
        for(int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;
            if(levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text = "" + levelGoals[i].numberNeeded + "/" + levelGoals[i].numberNeeded;
            }
            if(goalsCompleted >= levelGoals.Length)
            {
                if (endGame != null)
                {
                    if (endGame.currentCounterValue > 0 && board.currentState == GameState.wait)
                    {
                        StartCoroutine(endGame.WinGameAndMovesLeft());
                    }
                    //endGame.WinGame();
                }
                Debug.Log("U win");
            }
        }
    }

    public void CompareGoal(string goalToCompare)
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            if(goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }

    void ResetGoalsOnLevel()
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            levelGoals[i].numberCollected = 0;
        }
    }
}
