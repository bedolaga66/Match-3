using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public Button[] Levels;

    public int level;

    public GameData gameData;

    public string levelToLoad;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void LoadData()
    {
        if(gameData != null)
        {
            
            for (int i = 0; i < Levels.Length; i++)
            {
                if (gameData.saveData.isActive[i])
                {
                    Levels[i].interactable = true;
                }
                else
                {
                    Levels[i].interactable = false;
                }
            }
        }
    }

    public void level1Button()
    {
        level = 0;
    }

    public void level2Button()
    {
        level = 1;
    }

    public void level3Button()
    {
        level = 2;
    }

    public void level4Button()
    {
        level = 3;
    }

    public void level5Button()
    {
        level = 4;
    }

    public void level6Button()
    {
        level = 5;
    }

    public void Play()
    {
        PlayerPrefs.SetInt("Current Level", level);
        SceneManager.LoadScene(levelToLoad);
    }
}
