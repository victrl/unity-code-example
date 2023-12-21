using System;
using UnityEngine;

namespace App.Core.AppServices.ReadWriteSaveServices
{
    public class PlayerPrefsReadWriteSaveService : AppService, IReadWriteSaveService
    {
        private string PrefsKey = "PlayerPrefsReadWriteSaveSystem";

        public void ReadSaveData(Action<ISaveData> onComplete)
        {
            var saveData = PlayerPrefs.GetString(PrefsKey, string.Empty);
            onComplete?.Invoke(new SaveData(saveData));
        }

        public void WriteSaveData(string saveData, Action<bool> onComplete)
        {
            if (string.IsNullOrEmpty(saveData))
            {
                onComplete?.Invoke(false);
                return;
            }

            try
            {
                PlayerPrefs.SetString(PrefsKey, saveData);
                onComplete?.Invoke(true);
            }
            catch (Exception e)
            {
                Logger.LogWarning($"[PlayerPrefsReadWriteSaveService] => WriteSaveData: {e.GetInfo()}");
                onComplete?.Invoke(false);
            }
        }
    }
}