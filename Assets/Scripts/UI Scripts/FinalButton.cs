using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FinalButton : MonoBehaviour
{
    //public int levelIndex;

    private Button button;
    private GameData GameData;

    private void Start()
    {
        button = GetComponent<Button>();
        GameData = FindObjectOfType<GameData>();

        bool levelPassed = GameData.saveData.isActive[11];

        button.interactable = levelPassed;
    }
}
