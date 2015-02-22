using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

    public Vector3 r;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(r);
	}
}
