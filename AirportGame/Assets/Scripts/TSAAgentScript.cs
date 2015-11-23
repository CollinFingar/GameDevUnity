using UnityEngine;
using System.Collections;

public class TSAAgentScript : MonoBehaviour {
    public float speed = 10f;
    public float turnSpeed = 20f;

    public GameObject player;
    public float noticeDistance = 10f;

    public Vector3 velocity = new Vector3();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Vector3 targetDirection = player.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
        if (distanceToPlayer < noticeDistance && distanceToPlayer > 2 && angleToPlayer < 45) {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f));
            transform.position += transform.forward * speed * Time.deltaTime;
        }
	}
}
