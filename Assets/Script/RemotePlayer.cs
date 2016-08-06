using UnityEngine;
using System.Collections;

public class RemotePlayer : AbstractPlayerFsm 
{
	protected override void InitState ()
	{
		AddState (new RemoteWaitState ());
		AddState (new RemoteRunState ());
		AddState (new HitState ());

		gameObject.layer = LayerMask.NameToLayer("Enemy");
		GameObject.Destroy(GetComponentInChildren<BlockTrigger> ().gameObject);

		GotoState (StateName.Wait);
	}	
}
