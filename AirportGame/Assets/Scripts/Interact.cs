using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour {

    private float interactKey;
    private Camera fpsCamera;

    private float interactLength = 1f;

    void Start()
    {
        fpsCamera = GetComponentInChildren<Camera>();
        if (fpsCamera == null) { Debug.LogError("INTERACT: No camera component!"); }
    }

    void Update()
    {
        interactKey = Input.GetAxis("Interact");
        if (interactKey > .5f)
        {
            Vector3 cameraPosition = fpsCamera.transform.position;
            Vector3 cameraDirection = fpsCamera.transform.rotation * Vector3.forward;

            Ray ray = new Ray(cameraPosition, cameraDirection);
            //int tagLayer = 0;
            //RaycastHit hit;
            //bool didHit = Physics.Raycast(ray, out hit, interactLength, tagLayer);
            Debug.DrawRay(ray.GetPoint(0f), cameraDirection*interactLength, Color.red, 20f);
        }
    }

    void OnGUI()
    {
        var pos = new Vector2(8, 8);
        var size = new Vector2(1024, 32);
        GUI.Label(new Rect(pos, size), "char rotation: " + transform.position);
        pos.y += 16;
        GUI.Label(new Rect(pos, size), "cam rotation: " + fpsCamera.transform.position);
        pos.y += 16;
    }
}
