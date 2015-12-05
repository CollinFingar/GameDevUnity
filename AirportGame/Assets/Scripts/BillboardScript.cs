using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour {

    public GameObject player;

    public bool showingUp = false;
    public bool closingDown = false;

    private bool on = false;

    private float alpha = 0f;
    private SpriteRenderer sr;

    public Sprite twitter;
    public Sprite tumblr;
    public Sprite camera;

    public Vector3 twitterSize;
    public Vector3 tumblrSize;
    public Vector3 cameraSize;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, alpha);
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(player.transform.position, Vector3.up);
        Vector3 Heading = transform.rotation.eulerAngles;
        Heading.y += 180f;
        transform.rotation = Quaternion.Euler(Heading);
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            makeVisible("twitter");
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            makeVisible("tumblr");
        } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            makeVisible("camera");
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

    public void makeVisible(string media) {
        if (!on) {
            setMedia(media);
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

    void setMedia(string media) {
        if (media == "twitter") {
            sr.sprite = twitter;
            transform.localScale = twitterSize;
        } else if(media == "tumblr") {
            sr.sprite = tumblr;
            transform.localScale = tumblrSize;
        } else {
            sr.sprite = camera;
            transform.localScale = cameraSize;
        }
    }

}
