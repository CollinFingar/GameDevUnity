using UnityEngine;
using System.Collections;

public class InteractionGreenCube : Interaction {

    public Material red;
    public Material green;
    bool isRed;

    public override void BeInteractedWith()
    {
        if (isRed)
        {
            GetComponent<MeshRenderer>().material = green;
        }
        else
        {
            GetComponent<MeshRenderer>().material = red;
        }
        isRed = !isRed;
    }


}
