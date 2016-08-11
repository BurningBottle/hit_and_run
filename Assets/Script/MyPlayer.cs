using UnityEngine;
using System.Collections;

public class MyPlayer : AbstractPlayerFsm 
{
	protected override void InitState ()
	{
		AddState (new WaitState ());
		AddState (new RunState ());
		AddState (new HitState ());
		AddState (new DieState ());
		AddState (new VictoryState ());

		gameObject.layer = LayerMask.NameToLayer("Player");

		GotoState (StateName.Wait);
	}	

	protected override void SetHp (int hp)
	{
		base.SetHp (hp);
		GameUIManager.instance.SetHp (this.hp);
	}
}
