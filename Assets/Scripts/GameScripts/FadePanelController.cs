using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanelController : MonoBehaviour
{

    public Animator panelAnim;
    public Animator gameInfoAnim;
    private Board board;
    public Text noHealthText;

    public void OK()
    {
        if (PlayerPrefs.HasKey("CurrentHealth"))
        {
            string currHealth = PlayerPrefs.GetString("CurrentHealth");
            if (int.Parse(currHealth) != 0)
            {
                if (panelAnim != null && gameInfoAnim != null)
                {
                    panelAnim.SetBool("Out", true);
                    gameInfoAnim.SetBool("Out", true);
                    StartCoroutine(GameStartCo());
                    PlayerPrefs.SetString("CurrentHealth", (int.Parse(currHealth) - 1).ToString());
                }
            }
            else if(int.Parse(currHealth) == 0)
            {
                noHealthText.gameObject.SetActive(true);
            }
        }
    }

    public void GameOver()
    {
        panelAnim.SetBool("Out", false);
        panelAnim.SetBool("Game Over", true);
    }

    IEnumerator GameStartCo()
    {
        yield return new WaitForSeconds(1f);
        board = FindObjectOfType<Board>();
        board.currentState = GameState.move;
    }

    IEnumerator MenuStartCo()
    {
        yield return new WaitForSeconds(1f);
    }
}
