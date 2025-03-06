using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public XPData xpData;
    }

    public static string SaveFileName()
    {
        Debug.Log("Getting save file name.");
        string saveFile = Application.persistentDataPath + "/save" + ".data";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData));
    }

    private static void HandleSaveData()
    {
       // SaveManager.Instance.XPManager.Save(ref _saveData.xpData);
    }

    public static void Load()
    {
        if (File.Exists(SaveFileName()))
        {
            string saveContent = File.ReadAllText(SaveFileName());
            _saveData = JsonUtility.FromJson<SaveData>(saveContent);
            HandleLoadData();
        }
    }

    private static void HandleLoadData()
    {
       // SaveManager.Instance.XPManager.Load(_saveData.xpData);
    }
}
