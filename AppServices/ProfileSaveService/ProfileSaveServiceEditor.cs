using System.IO;
using App.Core.AppServices.ReadWriteSaveServices;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
namespace App.Core.AppServices
{
    public partial class ProfileSaveService
    {
        private const string commonPath = "App/Editor/Saves";

        [MenuItem("Save/Player Prefs/Export Save", false)]
        public static void ExportPrefsProfileData()
        {
            var readWriteService = new PlayerPrefsReadWriteSaveService();
            string path = Path.Combine(Application.dataPath, commonPath, "PrefsSave.txt");

            if (File.Exists(path) == false)
            {
                File.Create(path);
                Logger.Log($"[ProfileSaveService] => ImportPrefsProfileData: File was created");
            }

            readWriteService.ReadSaveData((saveData) =>
            {
                File.WriteAllText(path, saveData.RawData);
                EditorUtility.DisplayDialog("Profile Service", "Export was success", "ok");
            });
        }
        
        [MenuItem("Save/Player Prefs/Import Save", false)]
        public static void ImportPrefsProfileData()
        {
            var readWriteService = new PlayerPrefsReadWriteSaveService();
            string path = Path.Combine(Application.dataPath, commonPath, "PrefsSave.txt");

            
            if (File.Exists(path) == false)
            {
                EditorUtility.DisplayDialog("Profile Service", "File isn't exist", "ok");
                return;
            }
            
            var text = File.ReadAllText(path);

            readWriteService.WriteSaveData(text, (result) =>
            {
                if (result)
                {
                    EditorUtility.DisplayDialog("Profile Service", "Import was success", "ok");
                    return;
                }
                
                EditorUtility.DisplayDialog("Profile Service", "Import was failed", "ok");
            });
        }
    }
}
#endif