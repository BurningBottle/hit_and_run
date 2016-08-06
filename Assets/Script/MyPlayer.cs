using UnityEngine;
using System.Collections;

public class MyPlayer : AbstractPlayerFsm 
{
	protected override void InitState ()
	{
		AddState (new WaitState ());
		AddState (new RunState ());
		AddState (new HitState ());

		gameObject.layer = LayerMask.NameToLayer("Player");

		GotoState (StateName.Wait);
	}	
}
