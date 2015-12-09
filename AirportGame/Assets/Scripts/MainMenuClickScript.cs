using UnityEngine;
using System.Collections;

public class MainMenuClickScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
