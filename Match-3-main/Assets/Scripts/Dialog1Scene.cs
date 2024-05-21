using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialog1Scene : MonoBehaviour
{
    public void OnClickLoadScene2()
    {
        SceneManager.LoadScene(2);
    }
}
