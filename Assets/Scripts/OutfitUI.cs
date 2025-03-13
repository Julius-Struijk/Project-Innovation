using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitUI : MonoBehaviour
{
    [SerializeField] Renderer meshRenderer;
    [SerializeField] Material[] outfits;
    int prevOutfitIndex = 0;
    int currentOutfitIndex;

    public void ChangeOutfit(int outfitIndex)
    {
        currentOutfitIndex = outfitIndex;
        Debug.LogFormat("Changing outfit from {0} to {1}", meshRenderer.materials[2].name, outfits[currentOutfitIndex].name);
        // New material list is created to replace the existing one, so the materials actually change.
        Material[] updatedMaterials = new Material[meshRenderer.materials.Length];
        // First two materials (face and shader) stay the same.
        updatedMaterials[0] = meshRenderer.materials[0];
        updatedMaterials[1] = meshRenderer.materials[1];
        // Replaces previous outfit with new outfit.
        updatedMaterials[2] = outfits[currentOutfitIndex];
        meshRenderer.materials = updatedMaterials;
        Debug.LogFormat("Changed outfit to {0}", meshRenderer.materials[2].name);
    }

    public void CancelOutfit()
    {
        // To cancel a change it changes the outfit back to the previous one.
        ChangeOutfit(prevOutfitIndex);
    }

    public void ConfirmOutfit()
    {
        // Once the change is confirmed, the previous outfit is updated to be the new one.
        prevOutfitIndex = currentOutfitIndex;
    }

    #region

    public void Save(ref OutfitData data)
    {
        data.outfitData = prevOutfitIndex;
        //Debug.LogFormat("Saving outfit data. Outfit: {0}", outfits[data.outfitData].name);
    }

    public void Load(OutfitData data)
    {
        //Debug.LogFormat("Loading outfit data. Outfit: {0}", outfits[data.outfitData].name);
        prevOutfitIndex = data.outfitData;

        // Updating outfit to match loaded data
        ChangeOutfit(prevOutfitIndex);
    }

    #endregion
}

[System.Serializable]
public struct OutfitData
{
    public int outfitData;
}
