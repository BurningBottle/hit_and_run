using UnityEngine;
using System.Collections;

public class VictoryState : AbstractPlayerState  
{
	protected override void InitName ()
	{
		name = StateName.Victory;
	}

	public override void OnEnter ()
	{
		player.PlayAnimation ("Salute");
	}
}
