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
		player.ResetPath();
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
			player.MoveByInputDirection(joystickNormal);
		}
	}

	public override void OnEnd()
	{
		player.StopMove();
	}
}
