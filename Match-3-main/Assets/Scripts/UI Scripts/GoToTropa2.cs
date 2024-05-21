using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTropa2 : MonoBehaviour
{
    public Button GoToNextLocationButton;
    public Button GoPreviousLocationButton;
    public string SecondLocationName;
    public string FirstLocationName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GoToNextLoc()
    {
        SceneManager.LoadScene(SecondLocationName);
    }

    public void GoToPrevLoc()
    {
        SceneManager.LoadScene(FirstLocationName);
    }
}
