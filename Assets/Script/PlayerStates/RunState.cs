using UnityEngine;
using System.Collections;

public class RunState : AbstractPlayerState 
{
	float elapsedPacketInterval;

	protected override void InitName ()
	{
		name = StateName.Run;
	}

	public override void OnEnter ()
	{
		player.ResetPath();
		player.PlayAnimation ("Running(loop)");

		elapsedPacketInterval = 1000.0f;
    }

	public override void Update ()
	{
		var joystickNormal = VirtualJoystickRegion.VJRnormals;
        if (joystickNormal.magnitude == 0.0f) 
		{
			var packetData = new RunStartData();
			packetData.isStart = false;
			packetData.position = player.transform.position;
			MyNetworkManager.instance.SendReliable(new RunStartPacket(packetData));

			player.GotoState (StateName.Wait);
		}
		else
		{
			elapsedPacketInterval += Time.deltaTime;

            if (elapsedPacketInterval >= 0.06f)
			{
				var packetData = new KeyInputData();
				packetData.keyNormal = joystickNormal;
				MyNetworkManager.instance.SendReliable(new KeyInputPacket(packetData));

				elapsedPacketInterval = 0.0f;
			}

			player.MoveByInputDirection(joystickNormal);
		}
	}

	public override void OnEnd()
	{
		player.StopMove();
	}
}
