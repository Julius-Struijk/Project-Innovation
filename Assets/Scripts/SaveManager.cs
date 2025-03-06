using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
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
               // Instantiate(Resources.Load<SaveManager>("SaveManager"));
            }
//#endif

            //Debug.LogFormat("Returning instance. {0}", instance.name);
            return instance;
        }
    }

    // This is public so that it can be accessed by the save system;
    public XPManager XPManager { get; set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // When the application starts back up that's when it loads data.
        //SaveSystem.Load();
    }

    

    private void OnApplicationQuit()
    {
        // Saves data when the game is quit.
        //SaveSystem.Save();
    }
}
