using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour {

    public GameObject player;

    public bool showingUp = false;
    public bool closingDown = false;

    private bool on = false;

    private float alpha = 0f;
    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, alpha);
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position - player.transform.position, Vector3.up);
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            makeVisible();
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {

        }

        if (showingUp){
            if (alpha < 1) {
                alpha += .1f;
                sr.color = new Color(1, 1, 1, alpha);
            } else {
                showingUp = false;
            }
        }
        if (closingDown) {
            if (alpha > 0) {
                alpha -= .1f;
                sr.color = new Color(1, 1, 1, alpha);
            } else {
                closingDown = false;
            }
        }
    }

    void makeVisible() {
        if (!on) {
            on = true;
            showingUp = true;
            closingDown = false;
        }
        else {
            on = false;
            showingUp = false;
            closingDown = true;
        }
    }

}
