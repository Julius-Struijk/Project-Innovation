using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] XPManager xpManager;
    [SerializeField] OutfitUI outfitUI;
    [SerializeField] SceneLoading tutorial;
    // This allows saving and loading
    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
//#if UNITY_ANDROID
            if (!Application.isPlaying)
            {
                return null;
            }

            if (instance == null)
            {
                Instantiate(Resources.Load<SaveManager>("SaveManager"));
            }
//#endif

            return instance;
        }
    }

    // This is public so that it can be accessed by the save system;
    public XPManager XPManager { get; set; }
    public OutfitUI OutfitUI { get; set; }
    public SceneLoading Tutorial { get; set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Instance.XPManager = xpManager;
        Instance.OutfitUI = outfitUI;
        Instance.Tutorial = tutorial;
        // When the application starts back up that's when it loads data.
        SaveSystem.Load();
    }

    private void OnApplicationQuit()
    {
        SaveSystem.Save();
    }

    private void OnApplicationPause(bool pause)
    {
        // Saves data when the game is paused. This works better than if it's done when the application is quit.
        // Null check prevents it from trying to save data when the app starts up.
        if(Instance.XPManager != null) { SaveSystem.Save(); }
    }
}
