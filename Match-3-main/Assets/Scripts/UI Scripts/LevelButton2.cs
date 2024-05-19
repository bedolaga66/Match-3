using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton2 : MonoBehaviour
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
        if (gameData != null)
        {

            for (int i = 0; i < Levels.Length; i++)
            {
                if (gameData.saveData.isActive[i + 6])
                {
                    Levels[i].interactable = true;
                    //Levels[i].image.color = Color.yellow;
                    
                }
                else if (!gameData.saveData.isActive[i + 6])
                {
                    Levels[i].interactable = false;
                }
            }
            for (int i = 0; i < Levels.Length - 1; i++)
            {
                if (Levels[i + 1].interactable)
                {
                    Levels[i].image.color = Color.green;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    public void level7Button()
    {
        level = 6;
    }

    public void level8Button()
    {
        level = 7;
    }

    public void level9Button()
    {
        level = 8;
    }

    public void level10Button()
    {
        level = 9;
    }

    public void level11Button()
    {
        level = 10;
    }

    public void level12Button()
    {
        level = 11;
    }

    public void Play()
    {
        PlayerPrefs.SetInt("Current Level", level);
        SceneManager.LoadScene(levelToLoad);
    }
}
