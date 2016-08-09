using UnityEngine;
using System.Collections;

public class RemotePlayer : AbstractPlayerFsm 
{
	protected override void InitState ()
	{
		AddState (new RemoteWaitState ());
		AddState (new RemoteRunState ());
		AddState (new RemoteHitState ());

		gameObject.layer = LayerMask.NameToLayer("Enemy");
		//		GameObject.Destroy(GetComponentInChildren<BlockTrigger> ().gameObject);
		GetComponentInChildren<BlockTrigger>().isMine = false;

		MyNetworkManager.instance.RegisterReceiveNotifier(PacketId.RunStart, OnRunStart);
		MyNetworkManager.instance.RegisterReceiveNotifier(PacketId.Hit, OnReceiveHit);

		GotoState (StateName.Wait);
	}	

	void OnRunStart(byte[] data)
	{
		if (IsState(StateName.Die))
			return;

		var packetData = new RunStartPacket(data);
		var packet = packetData.GetPacket();
		transform.position = packet.position;

		GotoState(packet.isStart ? StateName.Run : StateName.Wait);
	}

	void OnReceiveHit(byte[] data)
	{
		if (IsState(StateName.Die))
			return;

		var packetData = new HitPacket(data);
		transform.position = packetData.GetPacket().position;

		GotoState(StateName.Hit);
	}
}
