using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOutfit : MonoBehaviour
{
    [SerializeField] Renderer meshRenderer;
    Material prevOutfit;

    private void Start()
    {
        prevOutfit = meshRenderer.materials[2];
    }

    //public void ChangeOutfit(Material newOutfit)
    //{
    //    Debug.LogFormat("Changing outfit from {0} to {1}", meshRenderer.materials[1].name, newOutfit.name);
    //    // Replaces previous outfit with new outfit.
    //    meshRenderer.materials[1] = newOutfit;
    //}

    public void ChangeOutfit(Material newOutfit)
    {
        Debug.LogFormat("Changing outfit from {0} to {1}", meshRenderer.materials[2].name, newOutfit.name);
        // New material list is created to replace the existing one, so the materials actually change.
        Material[] updatedMaterials = new Material[meshRenderer.materials.Length];
        // First two materials (face and shader) stay the same.
        updatedMaterials[0] = meshRenderer.materials[0];
        updatedMaterials[1] = meshRenderer.materials[1];
        // Replaces previous outfit with new outfit.
        updatedMaterials[2] = newOutfit;
        meshRenderer.materials = updatedMaterials;
        Debug.LogFormat("Changed outfit to {0}", meshRenderer.materials[2].name);
    }

    public void CancelOutfit()
    {
        // To cancel a change it changes the outfit back to the previous one.
        ChangeOutfit(prevOutfit);
    }

    public void ConfirmOutfit()
    {
        // Once the change is confirmed, the previous outfit is updated to be the new one.
        prevOutfit = meshRenderer.materials[2];
    }
}
