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
        public OutfitData outfitData;
        public TutorialData tutorialData;
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
        if(SaveManager.Instance.XPManager != null) { SaveManager.Instance.XPManager.Save(ref _saveData.xpData); }
        if (SaveManager.Instance.OutfitUI != null) { SaveManager.Instance.OutfitUI.Save(ref _saveData.outfitData); }
        if (SaveManager.Instance.Tutorial != null) { SaveManager.Instance.Tutorial.Save(ref _saveData.tutorialData); }
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
        if (SaveManager.Instance.XPManager != null) { SaveManager.Instance.XPManager.Load(_saveData.xpData); }
        if (SaveManager.Instance.OutfitUI != null) { SaveManager.Instance.OutfitUI.Load(_saveData.outfitData); }
        if (SaveManager.Instance.Tutorial != null) { SaveManager.Instance.Tutorial.Load(_saveData.tutorialData); }
    }
}
