using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uGUI_oFTextuer : MonoBehaviour {

    public UoF_OscServer uof;
    public RawImage rawImage;
	// Use this for initialization
	void Start () {
	    rawImage = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
        rawImage.texture = rawImage.material.mainTexture;
	}
}
