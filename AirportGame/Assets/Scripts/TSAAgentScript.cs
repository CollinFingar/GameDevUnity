using UnityEngine;
using System.Collections;

public class TSAAgentScript : MonoBehaviour {
    public float speed = 10f;
    public float turnSpeed = 20f;

    public GameObject player;
    public float noticeDistance = 10f;

    public bool followingPlayer = false;
    public bool movingBack = false;

    private Vector3 watchSpot = new Vector3();
    private Vector3 watchRotation = new Vector3();
    private Vector3 velocity = new Vector3();

    public Vector3 EXAMPLE_SPAWN_LOCATION = new Vector3(16, -.85f, 10);
    public GameObject itself;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        watchSpot = transform.position;
        watchRotation = transform.localEulerAngles;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canSee()) {
            Debug.Log("I SEE YOU");
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Vector3 targetDirection = player.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
        if (distanceToPlayer < noticeDistance && angleToPlayer < 45 && canSee()) {
            if (distanceToPlayer > 2) {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f));
                transform.position += transform.forward * speed * Time.deltaTime;
                followingPlayer = true;
            } else {
                Debug.Log("Gotcha");
                Application.LoadLevel(4);
            }
        } else {
            if (followingPlayer) {
                movingBack = true;
                followingPlayer = false;
            }
            if (movingBack)
            {
                targetDirection = watchSpot - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f));
                transform.position += transform.forward * speed * Time.deltaTime;
                if (Vector3.Distance(watchSpot, transform.position) < 2)
                {
                    movingBack = false;
                }
            }
            else {
                if (Mathf.Abs(transform.localEulerAngles.y - watchRotation.y) > 5)
                {
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 5, 0);
                }
                else {
                    transform.localEulerAngles = watchRotation;
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
            
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            spawnAgent(EXAMPLE_SPAWN_LOCATION);
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
	}

    void spawnAgent(Vector3 location) {
        GameObject newAgent = (GameObject)Instantiate(itself, location, Quaternion.identity);
        Vector3 targetDirection = player.transform.position - newAgent.transform.position;
        float angleToPlayer = Vector3.Angle(targetDirection, newAgent.transform.forward);
        newAgent.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angleToPlayer, transform.localEulerAngles.z);
    }

    bool canSee() {
        Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
        RaycastHit[] hit = Physics.RaycastAll(transform.position, playerPosition - transform.position, Vector3.Distance(transform.position, playerPosition));
        
        for (int i = hit.Length - 1; i > -1; i--)
        {
            if (hit[i].collider.gameObject.tag != "Player")
            {
                return false;
                
            }
        }
        Debug.DrawRay(transform.position, playerPosition - transform.position, Color.green);
        Debug.Log(hit.Length);
        return true;
        
    }

}
