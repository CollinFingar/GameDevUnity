using UnityEngine;
using System.Collections;

public class skipper : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Application.LoadLevel(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Application.LoadLevel(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            Application.LoadLevel(4);
        }
	}
}
