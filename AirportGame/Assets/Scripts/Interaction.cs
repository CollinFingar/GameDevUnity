using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {

    public virtual void BeInteractedWith()
    {
        Debug.Log("The defualt BeInteractedWith() was called. That probably shouldn't have happened.");
    }

	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}
}
