using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Chat : NetworkBehaviour {

	public static Chat chatManager;

	[SyncVar]
	public string m_chatLog = "";

	public static void SendMessage (string name, string msg) {
		if (msg != "" && msg != " ") {
			chatManager.SendMessage (name + " : " + msg + "\n");
		}
	}

	public void SendMessage(string msg) {
		if (isServer) {
			RpcSendMessage (msg);
		} else {
			CmdSendMessage (msg);
		}
		m_chatLog += msg;
	}
		

	[Command]
	public void CmdSendMessage (string msg) {
		RpcSendMessage (msg);
	}

	[ClientRpc]
	public void RpcSendMessage (string msg) {
		m_chatLog += msg;
	}

	// Use this for initialization
	void Start () {
		chatManager = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {
		GUI.TextArea (new Rect (Camera.main.pixelWidth - 300, Camera.main.pixelHeight - 300, 300, 300), m_chatLog);
	}
}
