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

	float elapsedTime;

	public override void Update ()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= 1.0f) 
		{
			elapsedTime = 0.0f;
			player.GotoState (StateName.Wait);
		}
	}
}
