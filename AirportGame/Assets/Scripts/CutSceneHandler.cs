using UnityEngine;
using System.Collections;

public class CutSceneHandler : MonoBehaviour {

    private ArrayList scenes;
    private int currentScene;

	// Use this for initialization
	void Start () {
        scenes = new ArrayList();
        foreach (Transform child in transform) {
            scenes.Add(child.gameObject);
        }
        currentScene = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            GameObject scene = (GameObject)scenes[currentScene];
            scene.SetActive(false);
            currentScene += 1;
            if (currentScene == scenes.Count) {
                Application.LoadLevel(2);
            }
        }
	}
}
