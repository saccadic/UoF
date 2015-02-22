using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityOSC;

using Debug = UnityEngine.Debug;

[System.Serializable]
public class oF_Options{
	public bool x86 = false;
	public bool windowtype_hidden = false;
	public string App_name = string.Empty;
	public string host = string.Empty;
	public    int RemotePort = 0;
	public    int ListenPort = 0;
}

class oF{
	public string App_name;
	private string app_path;
	private Process app_process;



	public void start_app(bool x86,bool windowtype_hidden,string App_name){
		
		this.App_name = App_name;
		Debug.Log ("start");
		if (x86 == true) {
			app_path = Application.streamingAssetsPath + "/.apps/x86/" + App_name + ".app";
		} else {
			app_path = Application.streamingAssetsPath + "/.apps/x64/" + App_name + ".app";
		}

		if (windowtype_hidden) {
			Process.Start (new ProcessStartInfo ("open", "-a '" + app_path + "'"){
				UseShellExecute = false
			});
		} else {
			Process.Start (new ProcessStartInfo ("open", "-g -a '" + app_path + "'"){
				UseShellExecute = false
			});
		}
	}
	
	public void kill_app(){
		Process.Start(new ProcessStartInfo ("killall",App_name){
			UseShellExecute = false 
		});
	}	
}

public class UoF : MonoBehaviour {

	public bool Run_App;
	public bool Use_Net;


	public oF_Options[] oF_opt;
	private oF[] of;



	public Dictionary<string,OscSocket> osc = new Dictionary<string, OscSocket>();

	void Awake() {
		of = new oF[oF_opt.Length];

		for(var i=0;i<oF_opt.Length;i++){
			of[i] = new oF();

			if(Run_App)
				of[i].start_app(oF_opt[i].x86,oF_opt[i].windowtype_hidden,oF_opt[i].App_name);

			if(Use_Net){
				this.osc[oF_opt[i].App_name] = new OscSocket();
				this.osc[oF_opt[i].App_name].Setup(oF_opt[i].host,oF_opt[i].RemotePort,oF_opt[i].ListenPort);
			}
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			OscMessage m = new OscMessage();
			m.addStringArg("test");
			osc[oF_opt[0].App_name].send(m);
		}
	}

	void OnApplicationQuit() {
		for(var i=0;i<oF_opt.Length;i++){
			osc[oF_opt[i].App_name].close();
			of[i].kill_app();
		}
	}
}
