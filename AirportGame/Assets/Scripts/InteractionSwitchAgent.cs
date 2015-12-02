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
            //Mesh tempMesh = GetComponent<MeshFilter>().mesh;
            GetComponent<MeshRenderer>().material = gO.GetComponent<MeshRenderer>().material;
            GetComponent<MeshFilter>().mesh= gO.GetComponent<MeshFilter>().mesh;
            gO.GetComponent<MeshRenderer>().material = tempMat;
        }

        /// Swap world position information
        {
            Vector3 tempPosition = transform.position;
            //Quaternion tempQuatn = transform.rotation;
            Vector3 gOGroundPosition;

            /// Raycast to get the position on the ground that this object is going to.
            {
                Ray ray = new Ray(gO.transform.position + new Vector3(0,1,0), Vector3.down);
                RaycastHit hit;
                bool didHit = Physics.Raycast(ray, out hit, 1 << LayerMask.NameToLayer("Floor"));
                if (!didHit) { Debug.LogError("SWITCH AGENT: Failed to find ground underneath player."); }
                gOGroundPosition = hit.point;
            }
            transform.position = gOGroundPosition + new Vector3(0,-0.01f,0);

            Vector3 playerEuler = gO.transform.rotation.eulerAngles;
            playerEuler.x = 0; // make sure the agent is standing up straight. 
            transform.rotation = Quaternion.Euler(playerEuler);

            gO.transform.position = tempPosition;
        }
    }

    void OnGUI()
    {
        //var pos = new Vector2(8, 8);
        //var size = new Vector2(1024, 32);
        //GUI.Label(new Rect(pos, size), "Debug Information: ");
        //pos.y += 16;
    }
}
