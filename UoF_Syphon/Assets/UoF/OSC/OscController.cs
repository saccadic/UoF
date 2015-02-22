using UnityEngine;
using System.Collections;
using UnityOSC;

public class OscController : MonoBehaviour {

    private OscSocket osc;

    public string host;
    public    int RemotePort;
    public    int ListenPort;

    // Use this for initialization
	void Start () {
        osc = new OscSocket();

        osc.Setup(host, RemotePort, ListenPort);

        osc.OnMassage += (soket,msg) => {
            Debug.Log(soket.Address);
            Debug.Log((string)msg.data[0]);
            //Debug.Log((long)msg.data[1]);
            //Debug.Log((float)msg.data[2]);
            //Debug.Log((string)msg.data[3]);
            //Debug.Log((byte[])msg.data[4]);
        };
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.Space)){
            var message = new OscMessage();
            message.setAdress("/from/unity/");
            //message.addIntArg(1);
            //message.addInt64Arg(1);
            //message.addFloatArg(9.9f);
            message.addStringArg("hogehoge");

            //byte[] data = System.Text.Encoding.ASCII.GetBytes("test");
            //message.addBlobArg(data);

            osc.send(message);
        }
	}

    void OnApplicationQuit()
    {
        osc.close();
    }
}
