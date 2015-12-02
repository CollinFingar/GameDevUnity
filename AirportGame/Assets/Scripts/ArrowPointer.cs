using UnityEngine;
using System.Collections;

public class ArrowPointer : MonoBehaviour {

    public GameObject waypoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        rotateTowardsWaypoint();
	}

    void rotateTowardsWaypoint() {
        //find the vector pointing from our position to the target
        Vector3 direction = (waypoint.transform.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
