using UnityEngine;
using System.Collections;

public class DieState : AbstractPlayerState  
{
	protected override void InitName ()
	{
		name = StateName.Die;
	}

	public override void OnEnter ()
	{
		player.PlayAnimation ("KneelDown");
	}
}
