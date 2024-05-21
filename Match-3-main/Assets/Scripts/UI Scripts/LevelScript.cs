using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public GameData gameData;
    private Board board;

    private bool isLevelCompleted;
    public bool levelCompleted;
    public int currLevel;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        gameData = FindObjectOfType<GameData>();
        
    }

    private void Update()
    {
        isNextLevelActive();
        
    }

    void isNextLevelActive()
    {

        if(board != null)
        {
            currLevel = board.level;
            if (board.world != null)
            {
                if (gameData != null)
                {
                    if (board.currentState == GameState.win)
                    {
                        isLevelCompleted = true;
                        lvlComplete();
                        //gameData.saveData.isActive[currLevel + 1] = true;
                    }
                    else
                    {
                        isLevelCompleted = false;
                        //gameData.saveData.isActive[currLevel + 1] = false;
                    }
                }
            }
        }
    }

    void lvlComplete()
    {
        if (isLevelCompleted)
        {
            levelCompleted = isLevelCompleted;
        }
        if (currLevel < 11)
        {
            gameData.saveData.isActive[currLevel + 1] = true;
            gameData.Save();
        }
    }
}
