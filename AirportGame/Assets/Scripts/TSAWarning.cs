using UnityEngine;
using System.Collections;

public class TSAWarning : MonoBehaviour {

    public int numberChasing = 0;
    private bool beingChased = true;
    private bool visible = false;

    private float timeToUpdate = 0f;

    public float interval = 1f;

    public Texture logo;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (numberChasing > 0) {
            updateWarning();
        } else {
            visible = false;
        }
	}

    void updateWarning() {
        if (Time.time > timeToUpdate) {
            if (visible) {
                visible = false;
                timeToUpdate = Time.time + interval;
            } else {
                visible = true;
                timeToUpdate = Time.time + interval;
            }
        }
    }
    void OnGUI() {
        if (visible) {
            GUI.color = new Color(1, 0, 0, .7f);
            GUI.DrawTexture(new Rect(30, 20, 120, 120), logo, ScaleMode.StretchToFill, true, 10.0F);
        }
    }
}
