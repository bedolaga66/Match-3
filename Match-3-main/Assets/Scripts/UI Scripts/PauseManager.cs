using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private Board board;
    public bool paused = false;
    public string newLevel;
    public Image musicButton;
    public Sprite MusicOnSprite;
    public Sprite MusicOffSprite;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound")==0)
            {
                musicButton.sprite = MusicOffSprite;
            }
            else
            {
                musicButton.sprite = MusicOnSprite;
            }
        }
        else
        {
            musicButton.sprite = MusicOnSprite;
        }
        pausePanel.SetActive(false);
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused && !pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            board.currentState = GameState.pause;
        }
        if (!paused && pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            board.currentState = GameState.move;
        }
    }

    public void SoundButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                musicButton.sprite = MusicOnSprite;
                PlayerPrefs.SetInt("Sound", 1);
            }
            else
            {
                musicButton.sprite = MusicOffSprite;
                PlayerPrefs.SetInt("Sound", 0);
            }
        }
        else
        {
            musicButton.sprite = MusicOffSprite;
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

    public void PauseGame() {
        paused = !paused;
    }
    public void ExitGame() {
        if(board.level >= 0 && board.level <= 5)
        {
            SceneManager.LoadScene(4);
        }
        if (board.level > 5 && board.level <= 11)
        {
            SceneManager.LoadScene(6);
        }
    }
}
