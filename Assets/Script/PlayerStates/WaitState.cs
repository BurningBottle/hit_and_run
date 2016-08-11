using UnityEngine;
using System.Collections;

public class WaitState : AbstractPlayerState 
{
	protected override void InitName ()
	{
		name = StateName.Wait;
	}

	public override void OnEnter ()
	{
		player.PlayAnimation ("Standing(loop)");
	}

	public override void Update ()
	{
		if (GameManager.instance.isGameOver)
			return;
		
		if (VirtualJoystickRegion.VJRnormals.magnitude > 0.0f)
		{
			var packetData = new RunStartData();
			packetData.isStart = true;
			packetData.position = player.transform.position;
			MyNetworkManager.instance.SendReliable(new RunStartPacket(packetData));

			player.GotoState(StateName.Run);
		}
	}
}
