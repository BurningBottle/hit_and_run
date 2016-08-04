using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TransportTcp
{
	Socket listner = null;
	Socket socket = null;
	Thread thread = null;

	PacketQueue sendQueue = new PacketQueue();
	PacketQueue recvQueue = new PacketQueue();

	NetEventHandler eventHandler = null;
	bool threadLoop = false;

	public bool isServer
	{
		get;
		private set;
	}

	public bool isConnected
	{
		get;
		private set;
	}

	public TransportTcp()
	{
		isServer = false;
		isConnected = false;
	}

	public bool StartServer(int port, int connectionNum)
	{
		listner = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		listner.Bind(new IPEndPoint(IPAddress.Any, port));
		listner.Listen(connectionNum);
		isServer = true;

		LaunchThread();

		return true;
	}

	public void StopServer()
	{
		if (listner == null)
			return;

		threadLoop = false;

		listner.Close();
		listner = null;
		isServer = false;
	}

	public bool Connect(string address, int port)
	{
		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		socket.NoDelay = true;
		socket.Connect(address, port);
		socket.SendBufferSize = 0;

		isConnected = true;

		if (eventHandler != null)
			eventHandler(new NetEventState(NetEventType.Connect, NetEventResult.Success));

		LaunchThread();

		return true;
	}

	public bool Disconnect()
	{
		isConnected = false;
		threadLoop = false;

		if (socket != null)
		{
			socket.Shutdown(SocketShutdown.Both);
			socket.Close();
			socket = null;
		}

		if (eventHandler != null)
			eventHandler(new NetEventState(NetEventType.Disconnect, NetEventResult.Success));

		return true;
	}

	public int Send(byte[] data, int size)
	{
		return sendQueue.Enqueue(data, size);
	}

	public int Receive(ref byte[] buffer, int size)
	{
		return recvQueue.Dequeue(ref buffer, size);
	}

	public void RegisterEventHandler(NetEventHandler handler)
	{
		eventHandler += handler;
	}

	public void UnregisterEventHandler(NetEventHandler handler)
	{
		eventHandler -= handler;
	}

	void AcceptClient()
	{
		if(listner != null && listner.Poll(0, SelectMode.SelectRead))
		{
			socket = listner.Accept();
			isConnected = true;

			if (eventHandler != null)
				eventHandler(new NetEventState(NetEventType.Connect, NetEventResult.Success));
		}
	}

	public void Dispatch()
	{
		while(threadLoop)
		{
			AcceptClient();

			if(socket != null && isConnected)
			{
				DispatchSend();
				DispatchReceive();
			}

			Thread.Sleep(5);
		}
	}

	void DispatchSend()
	{
		if(socket.Poll(0, SelectMode.SelectWrite))
		{
			var buffer = new byte[1024];
			int sendSize = sendQueue.Dequeue(ref buffer, buffer.Length);
			while(sendSize > 0)
			{
				socket.Send(buffer, sendSize, SocketFlags.None);
				sendSize = sendQueue.Dequeue(ref buffer, buffer.Length);
			}
		}
	}

	void DispatchReceive()
	{
		while(socket.Poll(0, SelectMode.SelectRead))
		{
			var buffer = new byte[1024];
			int recvSize = socket.Receive(buffer, buffer.Length, SocketFlags.None);

			if (recvSize == 0)
				Disconnect();
			else if (recvSize > 0)
				recvQueue.Enqueue(buffer, recvSize);
		}
	}

	bool LaunchThread()
	{
		try
		{
			threadLoop = true;
			thread = new Thread(new ThreadStart(Dispatch));
			thread.Start();
		}
		catch
		{
			Debug.LogError("Cannot launch thread!");
			return false;
		}

		return true;
	}
}
