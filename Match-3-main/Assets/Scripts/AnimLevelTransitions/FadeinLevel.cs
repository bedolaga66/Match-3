using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FadeinLevel : MonoBehaviour
{
    public Animator fadeInPanel;



    public void isButtonPressed()
    {
        if(fadeInPanel != null) {
            fadeInPanel.SetBool("ButtonPressed", true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
