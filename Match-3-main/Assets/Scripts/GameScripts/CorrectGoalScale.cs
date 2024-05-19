using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectGoalScale : MonoBehaviour
{
    public GameObject goalPrefab;
    public Image Image;
    public Text Text;
    private float scale = 1;

    // Start is called before the first frame update
    void Start()
    {
        goalPrefab.transform.localScale = new Vector3(scale, scale, scale);
        Image.transform.localScale = new Vector3 (scale, scale, scale);
        Text.transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
