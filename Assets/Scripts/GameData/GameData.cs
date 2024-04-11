using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{
    public bool[] isActive;
    public int[] highScores;
    public int[] stars;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;

    // Start is called before the first frame update
    void Awake()
    {
        if(gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Load();
    }

    private void Start()
    {
       
    }

    public void Save()
    {
        //Create a binary formatter which can read bin files
        BinaryFormatter formatter = new BinaryFormatter();
        
        //Create a route from program to file
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);
        
        //Create copy of save data
        SaveData data = new SaveData();
        data = saveData;
       
        //Save data in file & close data stream
        formatter.Serialize(file, data);
        file.Close();
        
        Debug.Log("saved");
    }

    public void Load()
    {
        //check game file exists
        if (File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            //create bin formatter
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("loaded");
        }
    }

    private void OnDisable()
    {
        Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
