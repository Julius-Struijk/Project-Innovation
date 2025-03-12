using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOutfit : MonoBehaviour
{
    [SerializeField] Renderer meshRenderer;
    [SerializeField] Material[] outfits;

    //public void ChangeOutfit(Material newOutfit)
    //{
    //    Debug.LogFormat("Changing outfit from {0} to {1}", meshRenderer.materials[1].name, newOutfit.name);
    //    // Replaces previous outfit with new outfit.
    //    meshRenderer.materials[1] = newOutfit;
    //}

    public void ChangeOutfit(int outfitIndex)
    {
        Debug.LogFormat("Changing outfit from {0} to {1} -----------------------------------------", meshRenderer.materials[2].name, outfits[outfitIndex].name);
        // Replaces previous outfit with new outfit.
        //meshRenderer.material = outfits[outfitIndex];
        meshRenderer.materials[2] = outfits[outfitIndex];
        //Debug.LogFormat("Changed outfit to {0}", meshRenderer.materials[1].name);
    }
}
