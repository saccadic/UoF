using UnityEngine;
using System.Collections;

public class array : MonoBehaviour {

	public GameObject obj;
	public int num;
	public Vector3 p;
	public Vector3 r;

	private GameObject[] clone;
	// Use this for initialization
	void Start () {
		clone = new GameObject[num];
		for(int i=0;i<num;i++){
			clone[i] = Instantiate(obj,p*i, Quaternion.Euler(r)) as GameObject;
			clone[i].transform.parent = gameObject.transform;
		}	
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0;i<num;i++){
			clone[i].transform.position = p*i + gameObject.transform.position;
		}
	}
}
