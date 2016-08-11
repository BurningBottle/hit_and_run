using UnityEngine;
using System.Collections;

public class RemotePlayer : AbstractPlayerFsm 
{
	protected override void InitState ()
	{
		AddState (new RemoteWaitState ());
		AddState (new RemoteRunState ());
		AddState (new RemoteHitState ());
		AddState (new DieState ());
		AddState (new VictoryState ());

		gameObject.layer = LayerMask.NameToLayer("Enemy");
		//		GameObject.Destroy(GetComponentInChildren<BlockTrigger> ().gameObject);
		GetComponentInChildren<BlockTrigger>().isMine = false;

		MyNetworkManager.instance.RegisterReceiveNotifier(PacketId.RunStart, OnRunStart);
		MyNetworkManager.instance.RegisterReceiveNotifier(PacketId.Hit, OnReceiveHit);

		GotoState (StateName.Wait);
	}	

	void OnRunStart(byte[] data)
	{
		if (IsState(StateName.Die) || GameManager.instance.isGameOver)
			return;

		var packetData = new RunStartPacket(data);
		var packet = packetData.GetPacket();
		transform.position = packet.position;

		GotoState(packet.isStart ? StateName.Run : StateName.Wait);
	}

	void OnReceiveHit(byte[] data)
	{
		if (IsState(StateName.Die) || GameManager.instance.isGameOver)
			return;

		var packetData = new HitPacket(data);
		transform.position = packetData.GetPacket().position;

		Damage ();

		if(hp == 0 && MyNetworkManager.instance.isServer)
			GameManager.instance.SendGameOver (this);
		else
			GotoState(StateName.Hit);
	}
}
