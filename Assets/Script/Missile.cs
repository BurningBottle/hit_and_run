using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	public GameObject drillFx;

	Vector3 direction;
	float elapsed;

	public void Shoot(Vector3 start, Vector3 dest)
	{
		direction = Vector3.Normalize (dest - start);
		transform.rotation = Quaternion.LookRotation (direction);

		Invoke ("DestroySelf", MyConst.missileLife);
	}

	void DestroySelf()
	{
		GameObject.Destroy (gameObject);
	}

	void Update () 
	{
		transform.position += direction * (Time.deltaTime * MyConst.missileVelocity);
		elapsed += Time.deltaTime;
		drillFx.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, elapsed * -3500.0f);
	}

	void OnTriggerEnter(Collider other)
	{
		if(string.Equals("Player", LayerMask.LayerToName(other.gameObject.layer)))
			other.SendMessage("OnHit");

		DestroySelf();
	}
}
