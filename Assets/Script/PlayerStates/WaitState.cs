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
		if (VirtualJoystickRegion.VJRnormals.magnitude > 0.0f)
			player.GotoState(StateName.Run);
	}
}
