using UnityEngine;
using System.Collections;

public class RunState : AbstractPlayerState 
{
	protected override void InitName ()
	{
		name = StateName.Run;
	}

	public override void OnEnter ()
	{
		player.PlayAnimation ("Running(loop)");
	}

	public override void Update ()
	{
		var joystickNormal = VirtualJoystickRegion.VJRnormals;
        if (joystickNormal.magnitude == 0.0f) 
		{
			player.GotoState (StateName.Wait);
		}
		else
		{
			var moveDir = new Vector3(joystickNormal.x, 0.0f, joystickNormal.y);
			moveDir.Normalize();

			var dest = player.transform.position + (moveDir * MyConst.playerMoveSpeed * Time.deltaTime);
			player.transform.position = dest;
		}
	}
}
