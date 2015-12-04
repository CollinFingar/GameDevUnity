using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour {

    public Texture aTexture;

    public bool showingUp = false;
    public bool closingDown = false;

    private bool on = false;
    private byte alpha = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            makeVisible();
        }

        if (showingUp)
        {
            if (alpha <  200)
            {
                alpha += 20;
            }
            else
            {
                showingUp = false;
            }
        }
        if (closingDown)
        {
            if (alpha > 0)
            {
                alpha -= 20;
            }
            else
            {
                closingDown = false;
            }
        }


    }

    void OnGUI() {
        if (on || (closingDown)) {
            GUI.color = new Color32(255, 255, 255, alpha);
            GUI.DrawTexture(new Rect(Screen.width - 400, 0, 400, 200), aTexture, ScaleMode.StretchToFill, true, 10.0F);
        }
        
    }


    void makeVisible()
    {
        if (!on)
        {
            on = true;
            showingUp = true;
            closingDown = false;
        }
        else
        {
            on = false;
            showingUp = false;
            closingDown = true;
        }
    }


}
