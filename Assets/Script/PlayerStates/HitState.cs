using UnityEngine;
using System.Collections;

public class HitState : AbstractPlayerState 
{
	float elapsedFrame = 0.0f;

	protected override void InitName ()
	{
		name = StateName.Hit;
	}

	public override void OnEnter ()
	{
		player.PlayAnimation ("Damaged(loop)");
		elapsedFrame = 0.0f;
		GameManager.instance.radialBlur.Show();

		var hitFxPosition = player.transform.position;
		hitFxPosition.y += 0.8f;
		GameManager.instance.CreateHitFx(hitFxPosition);

		var packetData = new HitData();
		packetData.position = player.transform.position;
		MyNetworkManager.instance.SendReliable(new HitPacket(packetData));
	}

	public override void Update ()
	{
		elapsedFrame += Time.deltaTime;
		if(elapsedFrame >= 1.0f)
			player.GotoState (StateName.Wait);
	}
}
