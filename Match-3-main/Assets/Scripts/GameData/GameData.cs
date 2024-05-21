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

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static SaveData FromJson(string json)
    {
        return JsonUtility.FromJson<SaveData>(json);
    }
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;

    //private void OnEnable() => YandexGame.LoadCloud();


    //// Start is called before the first frame update
    void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Load();

        //if (YandexGame.SDKEnabled)
        //{
        //    YandexGame.LoadCloud();
        //}
    }

    private void Start()
    {

    }

    //public void Save()
    //{
    //    //Create a binary formatter which can read bin files
    //    BinaryFormatter formatter = new BinaryFormatter();

    //    //Create a route from program to file
    //    FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);

    //    //Create copy of save data
    //    SaveData data = new SaveData();
    //    data = saveData;

    //    //Save data in file & close data stream
    //    formatter.Serialize(file, data);
    //    file.Close();

    //    Debug.Log("saved");
    //}

    //public void Load()
    //{
    //    //check game file exists
    //    if (File.Exists(Application.persistentDataPath + "/player.dat"))
    //    {
    //        //create bin formatter
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
    //        saveData = formatter.Deserialize(file) as SaveData;
    //        file.Close();
    //        Debug.Log("loaded");
    //    }
    //}

    public void Save()
    {
        string json = saveData.ToJson();
        File.WriteAllText(Application.persistentDataPath + "/player.json", json);
        Debug.Log("saved");


    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/player.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/player.json");
            saveData = SaveData.FromJson(json);
            Debug.Log("loaded");
        }
    }

    private void OnApplicationQuit()
    {
        Save();
        //if (YandexGame.SDKEnabled)
        //{
        //    YandexGame.SaveCloud();
        //}
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
