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
    private string menuScene;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        levelToLoad = SceneManager.GetActiveScene().name;
        menuScene = "TropaModified";
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
        SceneManager.LoadScene(menuScene);
    }
}
