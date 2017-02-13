using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientNetwork;
using autodemo;
public class ImageClientTest : MonoBehaviour {

	public string ipServer;
	public int portServer = 8888;
	// Use this for initialization
	NotifyManager notifyManager;
	ClientConnection imageClient;
	void Start () {
		notifyManager = gameObject.AddComponent<NotifyManager> ();
		notifyManager.RegisterMessageTrigger (OnReceveImage);

	}
	void ConnectToServer()
	{
		imageClient = new ClientConnection ();
	
	}
	void OnReceveImage(ImageMsg msg)
	{
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
