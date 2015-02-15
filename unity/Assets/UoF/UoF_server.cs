using UnityEngine;
using System.Collections;

using Unityof;
using WebSocketSharp;
using WebSocketSharp.Server;

public class UoF_server : MonoBehaviour {

    //UoF
    private UoF uof;

    //Websocket
    public string adress;
    public WebSocketServer wssv;

    //Options
    public string maessage;

    void Awake()
    {
        uof = gameObject.AddComponent<UoF>();
        wssv = new WebSocketServer(adress);
        wssv.Start();
    }

	// Use this for initialization
	void Start () {
	    //wssv.AddWebSocketService<>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
