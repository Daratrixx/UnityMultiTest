using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {
	
	[SyncVar]
	public string m_name;
	public string m_nameLabelContent;
	public string m_msgLabelContent;
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;
		
		var x = Input.GetAxis ("Horizontal") * 0.1f;
		var y = Input.GetAxis ("Vertical") * 0.1f;

		transform.Translate (x, y, 0);
	}

	[Command]
	public void CmdSetName (string name) {
		m_name = name;
	}

	public void SendMessage (string msg) {
		Chat.SendMessage (m_name, msg);
	}

	public override void OnStartLocalPlayer () {
		GetComponent<MeshRenderer> ().material.color = Color.red;
		if (isServer)
			CmdSetName ("Player (Server)");
		else
			CmdSetName ("Player (Client)");
	}

	void OnGUI () {
		if (isLocalPlayer) {
			m_nameLabelContent = GUI.TextField (new Rect (0, 200, 200, 50), m_nameLabelContent);
			if (GUI.Button (new Rect (0, 250, 200, 50), "GO!")) {
				CmdSetName (m_nameLabelContent);
			}
			m_msgLabelContent = GUI.TextField (new Rect (0, 300, 200, 50), m_msgLabelContent);
			if (GUI.Button (new Rect (0, 350, 200, 50), "GO!")) {
				SendMessage (m_msgLabelContent);
				m_msgLabelContent = "";
			}
		}
		Vector2 pos = Camera.main.WorldToScreenPoint (transform.localPosition);
		GUI.Label (new Rect (pos.x - 25, -pos.y + Camera.main.pixelHeight - 50, 200, 50), m_name);
	}

}
