using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOutfit : MonoBehaviour
{
    GameObject pet;
    GameObject currentOutfit;

    // Start is called before the first frame update
    void Start()
    {
        pet = GameObject.FindGameObjectWithTag("Pet");
    }

    public void ChangeOutfit(GameObject newOutfit)
    {
        if(currentOutfit != null) { Destroy(currentOutfit); }
        if(newOutfit != null) { currentOutfit = Instantiate(newOutfit, pet.transform.position, newOutfit.transform.rotation); }
    }
}
