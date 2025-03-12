using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public void MakeAppear(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void MakeDisappear(GameObject obj)
    {
        Debug.Log("deactivating");
        obj.gameObject.SetActive(false);
    }
}
