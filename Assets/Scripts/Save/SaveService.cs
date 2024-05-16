using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using YG;

namespace Match3.Save
{
    public class SaveService : MonoBehaviour
    {
        private const float AutoSaveInterval = 10f;

        [SerializeField] private Score _score;
        //[SerializeField] private PlayerData _playerData ;

        private ISaveSystem _saveSystem;

        #region MonoBehaviour

        private void OnEnable()
        {
            YandexGame.GetDataEvent += Load;
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent += Load;
        }

        private void Awake()
        {
            _saveSystem = new YandexGameSaveSystem();

            if (YandexGame.SDKEnabled == true)
            {
                Load();
                StartCoroutine(AutoSaveInterval());
            }
        }
    }
}
