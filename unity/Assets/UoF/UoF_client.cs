using UnityEngine;
using System.Collections;

using Unityof;
using WebSocketSharp;

public class UoF_client : MonoBehaviour {

    //UoF
    private UoF uof;

    //Websocket
    public string adress;
    public WebSocket ws;

    //Options
    public string message;
    public Texture2D img;
    public bool render = false;
    public byte[] data;
    void Awake()
    {
        uof = gameObject.GetComponent<UoF>();
        ws = new WebSocket(adress);
        ws.Connect();
        ws.Send("connect");
    }

	// Use this for initialization
	void Start () {
        

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("OnOpen:" + e);
        };
        ws.OnMessage += (sender, e) =>
        {
            switch(e.Type){
                case Opcode.Binary:
                    Debug.Log(e.RawData.Length);
                    data = e.RawData;
                    render = true;
                    break;
                case Opcode.Text:
                    Debug.Log(e.Data);
                    break;
            }
        };
        ws.OnError += (object sender, ErrorEventArgs e) =>
        {
            Debug.Log("OnError" + e.Message);
        };
        ws.OnClose += (object sender, CloseEventArgs e) =>
        {
            Debug.Log("OnClosed" + e.Reason);
        };
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyUp(KeyCode.Space)){
            ws.Send(message);
        }
        if (render)
        {
            render = false;
            img = uof.loadtexture(data, 150, 150);
            gameObject.renderer.material.mainTexture = img;
        }
	}

    void OnApplicationQuit()
    {
        if (ws.IsAlive)
        {
            ws.Close();
        }
    }
}