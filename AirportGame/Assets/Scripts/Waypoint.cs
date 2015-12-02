using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

    public GameObject player;
    public GameObject arrowObject;
    private ArrowPointer arrow;

    public GameObject nextWaypoint;

	// Use this for initialization
	void Start () {
        arrow = arrowObject.GetComponent<ArrowPointer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            arrow.waypoint = nextWaypoint;
        }
    }
}
