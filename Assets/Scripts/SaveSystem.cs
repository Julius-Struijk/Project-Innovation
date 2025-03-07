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
        //Debug.Log("Getting save file name.");
        string saveFile = Application.persistentDataPath + "/save" + ".data";
        return saveFile;
    }

    public static void Save()
    {
        Debug.Log("Saving progress.");
        HandleSaveData();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData));
    }

    private static void HandleSaveData()
    {
        if(SaveManager.Instance.XPManager == null) { Debug.Log("XPManager instance is null."); }
        SaveManager.Instance.XPManager.Save(ref _saveData.xpData);
    }

    public static void Load()
    {
        Debug.Log("Loading progress.");
        if (File.Exists(SaveFileName()))
        {
            string saveContent = File.ReadAllText(SaveFileName());
            _saveData = JsonUtility.FromJson<SaveData>(saveContent);
            HandleLoadData();
        }
    }

    private static void HandleLoadData()
    {
       SaveManager.Instance.XPManager.Load(_saveData.xpData);
    }
}
