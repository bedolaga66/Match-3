using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToFinalScene : MonoBehaviour
{
    public void OnClickLoadFinalScene()
    {
        SceneManager.LoadScene(7);
    }
}
