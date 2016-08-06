using UnityEngine;
using System.Collections;

public class RemoteWaitState : AbstractPlayerState 
{
	protected override void InitName ()
	{
		name = StateName.Wait;
	}

	public override void OnEnter ()
	{
		player.PlayAnimation ("Standing(loop)");
	}
}
