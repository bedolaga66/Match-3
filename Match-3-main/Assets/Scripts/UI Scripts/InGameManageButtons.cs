using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InGameManageButtons : MonoBehaviour
{
    private int level;
    private Board board;
    public Button[] buttonsToRestart;
    public Button[] buttonsToMainMenu;

    private string levelToLoad;
    private string menuScene1;
    private string menuScene2;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        levelToLoad = SceneManager.GetActiveScene().name;
        menuScene1 = "TropaModified";
        menuScene2 = "Tropa2Scene";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetCurrentLevel()
    {
        if(board != null)
        {
            if (board.world != null)
            {
                level = board.level;
            }
        }
    }

    public void RestartLevel()
    {
        GetCurrentLevel();
        PlayerPrefs.SetInt("Current Level", level);
        SceneManager.LoadScene(levelToLoad);
    }

    public void BackToMenu()
    {
        GetCurrentLevel();
        if(level >= 0 && level < 5)
        {
            SceneManager.LoadScene(menuScene1);
        }
        else if(level >= 5 && level <= 11) SceneManager.LoadScene(menuScene2);
    }

    public void BackToMenuFromIntroGoals()
    {
        GetCurrentLevel();
        if (level >= 0 && level <= 5)
        {
            SceneManager.LoadScene(menuScene1);
        }
        else if (level > 5 && level <= 11) SceneManager.LoadScene(menuScene2);
    }

    public void BackToMenuFromLosePanel()
    {
        GetCurrentLevel();
        if (level >= 0 && level <= 5)
        {
            SceneManager.LoadScene(menuScene1);
        }
        else if (level > 5 && level <= 11) SceneManager.LoadScene(menuScene2);
    }
}
