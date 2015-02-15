using UnityEngine;
using System.Collections;

public class make_box : MonoBehaviour {

	public GameObject obj;
	public Vector3 num;
	public int n=0;
	public float offset = 0;
	// Use this for initialization
	void Start () {
		for(int z=0;z<(int)num.z;z++){
			for(int y=0;y<(int)num.y;y++){
				for(int x=0;x<(int)num.x;x++){
					GameObject clone = (GameObject)Instantiate(obj);
					clone.transform.parent = transform;
					clone.transform.position = new Vector3(x,y,z) * offset + transform.position;
					clone.gameObject.name = n+"";
					n++;
				}
			}
		}
	}
}
