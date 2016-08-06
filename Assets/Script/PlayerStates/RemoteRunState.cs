using UnityEngine;
using System.Collections;

public class RemoteRunState : AbstractPlayerState 
{
	Vector2 remoteKeyNormal;

	protected override void InitName ()
	{
		name = StateName.Run;

		MyNetworkManager.instance.RegisterReceiveNotifier(PacketId.KeyInput, OnKeyInput);		
	}

	void OnKeyInput(byte[] data)
	{
		var packetData = new KeyInputPacket(data);
		remoteKeyNormal = packetData.GetPacket().keyNormal;
	}

	public override void OnEnter ()
	{
		player.PlayAnimation ("Running(loop)");
	}

	public override void Update ()
	{
		player.MoveByInputDirection(remoteKeyNormal);
	}
}
