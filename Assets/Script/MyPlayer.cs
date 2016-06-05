using UnityEngine;
using System.Collections;

public class MyPlayer : AbstractPlayerFsm 
{
	protected override void InitState ()
	{
		AddState (new WaitState ());
		AddState (new RunState ());

		GotoState (StateName.Wait);
	}	
}
