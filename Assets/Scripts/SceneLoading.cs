using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    // Check to see if the tutorial has already been completed.
    bool tutorialCompleted = false;

    public void LoadScene(string sceneName)
    {
        if(sceneName == "Tim Scene") { 
            tutorialCompleted = true;
            // A save is forced so that the data is always saved.
            SaveSystem.Save();
        }
        SceneManager.LoadScene(sceneName);
    }

    #region

    public void Save(ref TutorialData data)
    {
        data.tutorialData = tutorialCompleted;
        Debug.LogFormat("Saving tutorial data. Tutorial completed: {0}", data.tutorialData);
    }

    public void Load(TutorialData data)
    {
        Debug.LogFormat("Loading tutorial data. Tutorial completed: {0}", data.tutorialData);
        tutorialCompleted = data.tutorialData;

        // Skipping the tutorial if it's already been completed.
        if (tutorialCompleted) { LoadScene("Tim Scene"); }
    }

    #endregion
}

[System.Serializable]
public struct TutorialData
{
    public bool tutorialData;
}
