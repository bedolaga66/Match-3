using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton1 : MonoBehaviour
{
    public Button Level1;
    public Button Level2;
    public Button Level3;
    public Button Level4;
    public Button Level5;
    public Button Level6;
    int levelComplete;

    private Board board;
    private GameData gameData;

    //public void isLevelCompleted()
    //{
    //    if(board.currentState == GameState.win && gameData != null)
    //    {
    //        gameData.saveData.isActive
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        gameData = FindObjectOfType<GameData>();
        levelComplete = PlayerPrefs.GetInt("LevelComplete");
        Level2.interactable = false;
        Level3.interactable = false;
        Level4.interactable = false;
        Level5.interactable = false;
        Level6.interactable = false;

        switch (levelComplete){
            case 1:
                Level2.interactable = true;
                break;
            case 2:
                Level3.interactable = true;
                break;
            case 3:
                Level4.interactable = true;
                break;
            case 4:
                Level5.interactable = true;
                break;
            case 5: 
                Level6.interactable = true;
                break;
        }
    }
    

    public void LoadTo(int Level)
    {
        SceneManager.LoadScene(Level);   
    }
}
