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

	float elapsedTime = 0.0f;

	public override void Update ()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= 1.0f) 
		{
			elapsedTime = 0.0f;
			player.GotoState (StateName.Run);
		}
	}
}
