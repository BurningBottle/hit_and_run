﻿using UnityEngine;
using System.Collections;

public enum NetEventType
{
	Connect = 0,
	Disconnect,
	SendError,
	ReceiveError,
}

public enum NetEventResult
{
	Failure = -1,
	Success = 0,
}

public class NetEventState
{
	public NetEventType type;
	public NetEventResult result;

	public NetEventState(NetEventType type, NetEventResult result)
	{
		this.type = type;
		this.result = result;
	}
}

public delegate void NetEventHandler(NetEventState state);