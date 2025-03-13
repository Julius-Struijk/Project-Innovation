using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    bool roomIsDark = false;

    public void ToggleDarkness()
    {
        if (roomIsDark)
        {
            roomIsDark = false;
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
            roomIsDark = true;
        }
    }

}
