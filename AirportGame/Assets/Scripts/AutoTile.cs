using UnityEngine;
using System.Collections;

public enum Dir {X, Y, Z };
public class AutoTile : MonoBehaviour {

    public Dir dir;
    public float size = 1f;
	// Use this for initialization
	void Start () {
        Vector2 scale = new Vector2(1, 1);
        switch (dir)
        {
            case (Dir.X):
                {
                    scale.x = transform.lossyScale.z;
                    scale.y = transform.lossyScale.y;
                }
                break;
            case (Dir.Y):
                {
                    scale.x = transform.lossyScale.x;
                    scale.y = transform.lossyScale.z;
                }
                break;
            case (Dir.Z):
                {
                    scale.x = transform.lossyScale.x;
                    scale.y = transform.lossyScale.y;
                }
                break;
        }
        scale *= (1f / size);
        GetComponent<Renderer>().material.mainTextureScale = scale;
    }

    void OnDrawGizmos()
    {
        Vector2 scale = new Vector2(1,1);
        switch (dir)
        {
            case (Dir.X):
                {
                    scale.x = transform.lossyScale.z;
                    scale.y = transform.lossyScale.y;
                }
                break;
            case (Dir.Y):
                {
                    scale.x = transform.lossyScale.x;
                    scale.y = transform.lossyScale.z;
                }
                break;
            case (Dir.Z):
                {
                    scale.x = transform.lossyScale.x;
                    scale.y = transform.lossyScale.y;
                }
                break;
        }
        scale *= (1f/size);
        GetComponent<Renderer>().material.mainTextureScale = scale;
    }
    
}
