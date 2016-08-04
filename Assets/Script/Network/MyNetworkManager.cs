using UnityEngine;
using System.Collections;

public class MyNetworkManager : MonoBehaviour
{
	TransportTcp transTcp= new TransportTcp();

	public static MyNetworkManager instance = null;

	public string serverAddress
	{
		get;
		private set;
	}

	public int port
	{
		get;
		private set;
	}

	public bool isServer
	{
		get
		{
			return transTcp.isServer;
		}
	}

	public bool isConnected
	{
		get
		{
			return transTcp.isConnected;
		}
	}

	void Awake()
	{
		MyNetworkManager.instance = this;
		GameObject.DontDestroyOnLoad(gameObject);

		transTcp.RegisterEventHandler(OnNetworkEvent);
		port = 8000;
    }

	void OnDestroy()
	{
		MyNetworkManager.instance = null;
	}

	public void StartServer()
	{
		serverAddress = Network.player.ipAddress;
		transTcp.StartServer(port, 2);
	}

	public void Connect(string ipAddress)
	{
		serverAddress = ipAddress;
		transTcp.Connect(serverAddress, port);
	}

	void OnApplicationQuit()
	{
		if(isConnected)
		{
			if (isServer)
				transTcp.StopServer();
			else
				transTcp.Disconnect();
		}
	}

	void OnNetworkEvent(NetEventState state)
	{
		switch(state.type)
		{
			case NetEventType.Connect:
				Debug.Log("Connect : " + state.result);
				break;

			case NetEventType.Disconnect:
				Debug.Log("Disconnect : " + state.result);
				break;
		}
	}
}
