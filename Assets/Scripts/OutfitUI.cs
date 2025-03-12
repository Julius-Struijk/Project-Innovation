using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOutfit : MonoBehaviour
{
    GameObject pet;
    GameObject currentOutfit;
    [SerializeField] GameObject customizationUI;

    // Start is called before the first frame update
    void Start()
    {
        pet = GameObject.FindGameObjectWithTag("Pet");
        customizationUI.SetActive(false);
    }

    public void ChangeOutfit(GameObject newOutfit)
    {
        Debug.Log("Changing outfit.");
        if(currentOutfit != null) { Destroy(currentOutfit); }
        if(newOutfit != null) { currentOutfit = Instantiate(newOutfit, pet.transform.position, newOutfit.transform.rotation); }
        customizationUI.SetActive(false);
    }

    public void EnableUI() { customizationUI.SetActive(true); }
}
