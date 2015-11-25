using UnityEngine;
using System.Collections;

public class InteractionSwitchAgent : Interaction {

    public override void BeInteractedWith(GameObject gO)
    {
        //Debug.Log("Swap Mesh");
        /// Swap Mesh and Materials
        /// 
        /// If you are looking at this now, it is probably because the animations aren't
        /// swapping right. It's because you've importanted austin's meshes and animations
        /// unity needs you to mess with the SkinnedMeshRenderer instead of MeshRenderer.
        /// There will be other complications.
        {
            Material tempMat = GetComponent<MeshRenderer>().material;
            Mesh tempMesh = GetComponent<MeshFilter>().mesh;
            GetComponent<MeshRenderer>().material = gO.GetComponent<MeshRenderer>().material;
            GetComponent<MeshFilter>().mesh= gO.GetComponent<MeshFilter>().mesh;
            gO.GetComponent<MeshRenderer>().material = tempMat;
            gO.GetComponent<MeshFilter>().mesh = tempMesh;
        }

        /// Swap world position information
        {
            //Debug.Log("Camera Rotation: "+gO.GetComponentInChildren<Camera>().transform.rotation);

            Vector3 tempPosition = transform.position;
            Quaternion tempQuatn = transform.rotation;
            //Debug.Log("object rotation: " + tempQuatn);
            transform.position = gO.transform.position;

            Vector3 playerEuler = gO.transform.rotation.eulerAngles;
            playerEuler.x = 0; // make sure the agent is standing up straight. 
            transform.rotation = Quaternion.Euler(playerEuler);

            gO.transform.position = tempPosition;
            //gO.GetComponentInChildren<Camera>().transform.rotation = tempQuatn;
            //gO.transform.rotation = tempQuatn;
            //Debug.Log("object rotation2: " + tempQuatn);
        }
    }

    void OnGUI()
    {
        var pos = new Vector2(8, 8);
        var size = new Vector2(1024, 32);
        //GUI.Label(new Rect(pos, size), "Debug Information: ");
        //pos.y += 16;
    }
}
