using UnityEngine;
using System.Collections;

public class MyPlayer : AbstractPlayerFsm 
{
	protected override void InitState ()
	{
		AddState (new WaitState ());
		AddState (new RunState ());
		AddState (new HitState ());

		GotoState (StateName.Wait);
	}	
}
