using UnityEngine;
using System;
using System.Diagnostics;
using System.Collections;

using Debug = UnityEngine.Debug;

public class run_app : MonoBehaviour {

	public string App_name;
	public bool windowtype_hidden = false;
	public bool x86;

	public bool StartApp;
	public string app_path;
	private Process app_process;

	void Awake() {

	}

	// Use this for initialization
	void Start () {
		StartApp = start_app (x86,windowtype_hidden,App_name);
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnApplicationQuit() {
		if (StartApp) {
			//kill_app ();
		}
	}
	
	public bool start_app(bool arch,bool windowtype_hidden,string App_name){
		this.x86 = arch;
		this.windowtype_hidden = windowtype_hidden;
		
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
		return true;
	}
	
	public void kill_app(string App_name){
		Process.Start(new ProcessStartInfo ("killall",App_name){
			UseShellExecute = false 
		});
	}
}
