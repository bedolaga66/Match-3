using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using YG;

public class YandexGameSaveSystem : ISaveSystem
{
    public void Save(SaveData data)
    {
        YandexGame.savesData += data;
        YandexGame.SaveProgress();
    }

    public SaveData Load()
    {
        return (SaveData)YandexGame.savesData;
    }
}
