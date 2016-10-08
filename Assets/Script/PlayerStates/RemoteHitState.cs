using UnityEngine;
using System.Collections;

public class RemoteHitState : AbstractPlayerState
{
	float elapsedFrame = 0.0f;

	protected override void InitName()
	{
		name = StateName.Hit;
	}

	public override void OnEnter()
	{
		player.PlayAnimation("Damaged(loop)");
		elapsedFrame = 0.0f;

		var hitFxPosition = player.transform.position;
		hitFxPosition.y += 0.8f;
		GameManager.instance.CreateHitFx(hitFxPosition);
	}

	public override void Update()
	{
		elapsedFrame += Time.deltaTime;
		if (elapsedFrame >= 1.0f)
			player.GotoState(StateName.Wait);
	}
}
