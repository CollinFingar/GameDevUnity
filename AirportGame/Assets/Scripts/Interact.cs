using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour {

    private bool interactKey;
    private bool interactJustPressed;
    private Camera fpsCamera;

    private float interactLength = 1f;

    void Start()
    {
        fpsCamera = GetComponentInChildren<Camera>();
        if (fpsCamera == null) { Debug.LogError("INTERACT: No camera component!"); }
    }

    void Update()
    {
        bool frameInteractKey = Input.GetAxis("Interact") > .5f;

        interactJustPressed = false;
        if (interactKey != frameInteractKey)
        {
            interactKey = frameInteractKey;
            if (interactKey) { interactJustPressed = true; }
        }
        
        if (interactJustPressed)
        {
            Vector3 cameraPosition = fpsCamera.transform.position;
            Vector3 cameraDirection = fpsCamera.transform.rotation * Vector3.forward;

            Ray ray = new Ray(cameraPosition, cameraDirection);
            RaycastHit hit;
            bool didHit = Physics.Raycast(ray, out hit, interactLength, 1 << LayerMask.NameToLayer("Interactable"));

            //Color col;
            if (didHit)
            {
                //col = Color.green;
                if (hit.transform == null)
                { Debug.LogError("INTERACT: Raycast return null transform."); }
                Interaction Interactable = hit.transform.GetComponent<Interaction>();
                Interactable.BeInteractedWith(gameObject);
            }
            else
            {
                //col = Color.red;
            }
            //Debug.DrawLine(ray.origin, ray.GetPoint(interactLength), col, 20f);
        }
    }

    void OnGUI()
    {
        var pos = new Vector2(8, 8);
        //var size = new Vector2(1024, 32);
        //GUI.Label(new Rect(pos, size), "Debug Information: "+GetComponentInChildren<Camera>().transform.rotation.eulerAngles);
        //pos.y += 16;
    }
}
