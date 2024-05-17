using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Text health;
    float timer1 = 0f;
    public float startingTime;
    public string timeString;
    public string timeExitString;
    public Text timerText;
    public bool isCoroutineRunning;
    public float timeElapsed = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        GetCurrentHealth();
        
    }

    void Start()
    {
        isCoroutineRunning = false;
        timeElapsed = 0f;
        //GetCurrentTime();
        //string t1 = PlayerPrefs.GetString("ExitTime");
        //string t2 = PlayerPrefs.GetString("CurrentTime");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCoroutineRunning)
        {
            StartTimer();
        }
        //PlayerPrefs.SetString("CurrentTime", DateTime.Now.ToString());
    }

    void OnApplicationQuit()
    {
        //PlayerPrefs.SetString("ExitTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("CurrentHealth", health.text);
        PlayerPrefs.SetFloat("TimeLeft", timer1);
        PlayerPrefs.SetString("TimeOut", DateTime.Now.ToString());
    }

    public void GetCurrentHealth()
    {
        string healthCurr = PlayerPrefs.GetString("CurrentHealth");
        if (healthCurr == "")
        {
            health.text = "" + 5.ToString();
        }
        else if(healthCurr != "")
        {
            health.text = PlayerPrefs.GetString("CurrentHealth");
        }
    }

    public void GetTimerTime()
    {
        if(PlayerPrefs.GetFloat("TimeLeft") == 0f)
        {
            return;
        }
        else if(PlayerPrefs.GetFloat("TimeLeft") != 0f)
        {
            timer1 = PlayerPrefs.GetFloat("TimeLeft");
        }
    }

    public void GetElapsedTime()
    {
        if (PlayerPrefs.GetString("TimeOut") != "")
        {
            DateTime exitTime = DateTime.Parse(PlayerPrefs.GetString("TimeOut"));
            TimeSpan timeDifference = DateTime.Now - exitTime;
            timeElapsed = (float)timeDifference.TotalSeconds;
        }
        else
        {
            timeElapsed = 0f;
        }
    }

    public void StartTimer()
    {
        StartCoroutine(IncreaseHealth());
    }
    IEnumerator IncreaseHealth()
    {
        
        if (int.Parse(health.text) < 5 && int.Parse(health.text) >= 0)
        {
            isCoroutineRunning = true;
            StartCoroutine(Timer());
            yield return new WaitForSeconds(startingTime);
            
            //PlayerPrefs.SetString("CollectedCoins", DateTime.Now.ToString());
        }
        else if(int.Parse(health.text) == 5) {
            isCoroutineRunning = false;
            yield break;
        }
        isCoroutineRunning = false;
    }
    public IEnumerator Timer()
    {
        GetElapsedTime();
        GetTimerTime();
        if (PlayerPrefs.GetFloat("TimeLeft") != 0f)
        {
            if (timer1 > timeElapsed)
            {
                timer1 -= timeElapsed;
            }
            else if (timer1 == timeElapsed)
            {
                timer1 = startingTime;
            }
            else if (timer1 < timeElapsed)
            {
                float tTimeElapsed = timeElapsed / startingTime;
                if (tTimeElapsed == 5 || tTimeElapsed > 5)
                {
                    health.text = 5.ToString();
                    timer1 = 0f;
                    yield break;
                }
                else if (tTimeElapsed < 5)
                {
                    int currHealth = int.Parse(health.text);
                    currHealth += (int)tTimeElapsed;
                    if (currHealth == 5 || currHealth > 5)
                    {
                        health.text = 5.ToString();
                        yield break;
                    }
                    else
                    {
                        health.text = currHealth.ToString();
                    }
                }
            }
        }
        else
        {
            timer1 = startingTime;
        }
        do
        {
            timer1 -= Time.deltaTime;

            FormatText1();

            yield return null;


        }
        while (timer1 > 0);
        health.text = (int.Parse(health.text) + 1).ToString();

        
    }

    public void FormatText1()
    {

        int minutes = (int)(timer1 / 60) % 60;
        int seconds = (int)(timer1 % 60);
        if (minutes >= 10 && seconds >= 10)
        {
            timeString = "" + minutes.ToString() + ":" + seconds.ToString();
            timerText.text = "" + minutes.ToString() + ":" + seconds.ToString();
        }
        else if (minutes >= 10 && seconds < 10)
        {
            timeString = "" + minutes.ToString() + ":" + "0" + seconds.ToString();
            timerText.text = "" + minutes.ToString() + ":" + "0" + seconds.ToString();
        }
        else if (minutes < 10 && seconds < 10)
        {
            timeString = "0" + minutes.ToString() + ":" + "0" + seconds.ToString();
            timerText.text = "0" + minutes.ToString() + ":" + "0" + seconds.ToString();
        }
        else if (minutes < 10 && seconds >= 10)
        {
            timeString = "0" + minutes.ToString() + ":" + "" + seconds.ToString();
            timerText.text = "0" + minutes.ToString() + ":" + "" + seconds.ToString();
        }
    }
}
