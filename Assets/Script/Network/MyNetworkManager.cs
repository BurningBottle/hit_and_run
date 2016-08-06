using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class MyNetworkManager : MonoBehaviour
{
	TransportTcp transTcp= new TransportTcp();

	public static MyNetworkManager instance = null;

	enum State
	{
		WaitForConnect,
		Connected,
		InGame,
		GameOver,
	}

	public delegate void RecvNotifier(byte[] data);

	State managerState = State.WaitForConnect;
	Dictionary<PacketId, RecvNotifier> notifierTable = new Dictionary<PacketId, RecvNotifier>();

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
		port = 25331;
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
				if (state.result == NetEventResult.Success && managerState == State.WaitForConnect)
					managerState = State.Connected;
				break;

			case NetEventType.Disconnect:
				Debug.Log("Disconnect : " + state.result);
				break;
		}
	}

	void Update()
	{
		if(isConnected)
		{
			byte[] packet = new byte[1024];

			while(transTcp.Receive(ref packet, packet.Length) > 0)
			{
				ReceivePacket(packet);
			}
		}

		switch(managerState)
		{
			case State.Connected:
				managerState = State.InGame;
				SceneManager.LoadScene("Main");
				break;
		}
	}

	public void ReceivePacket(byte[] data)
	{
		PacketHeader header = new PacketHeader();
		HeaderSerializer serializer = new HeaderSerializer();

		serializer.Deserialize(data, ref header);

		int headerSize = sizeof(int);
		byte[] packetData = new byte[data.Length - headerSize];
		System.Buffer.BlockCopy(data, headerSize, packetData, 0, packetData.Length);

		if (notifierTable.ContainsKey(header.packetId))
			notifierTable[header.packetId](packetData);

		var packetSerializer = new LoadingCompletePacket(packetData);

		Debug.Log("RecvPacket : [" + header.packetId + "] " + packetSerializer.GetPacket().playerId);
    }

	public int SendReliable<T>(IPacket<T> packet)
	{
		int sendSize = 0;

		var header = new PacketHeader();
		var serializer = new HeaderSerializer();

		header.packetId = packet.GetPacketId();

		byte[] headerData = null;
		if (!serializer.Serialize(header))
			return 0;

		headerData = serializer.GetSerializedData();

		var packetData = packet.GetData();
		var data = new byte[headerData.Length + packetData.Length];

		int headerSize = Marshal.SizeOf(typeof(PacketHeader));
		System.Buffer.BlockCopy(headerData, 0, data, 0, headerSize);
		System.Buffer.BlockCopy(packetData, 0, data, headerSize, packetData.Length);

		sendSize = transTcp.Send(data, data.Length);

		Debug.Log("SendPacket : [" + packet.GetPacketId() + "]");

		return sendSize;
	}
}
