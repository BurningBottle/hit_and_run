using UnityEngine;
using System.Collections;

public class RemotePlayer : AbstractPlayerFsm 
{
	protected override void InitState ()
	{
		AddState (new RemoteWaitState ());
		AddState (new RemoteRunState ());
		AddState (new HitState ());

		GameObject.Destroy(GetComponentInChildren<BlockTrigger> ());

		GotoState (StateName.Wait);
	}	
}
