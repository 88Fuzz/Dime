using UnityEngine;

public class LayerUtils
{
    /*
     * Returns true if the numbered layer i.e. non-bitshifted layer is the same as the LayerMask.
     * For example the Ignore Raycast layer number is 2, and if compared with the LayerMast 4, will return true.
     */
    //TODO I sure as hell hope Unity provides a tool to already do this. WHY WHOULD THEY NOT!??!!?
    public static bool CompareLayerWithLayerMask(int layer, LayerMask layerMask)
    {
        return ((1 << layer) & layerMask.value) != 0;
    }
}