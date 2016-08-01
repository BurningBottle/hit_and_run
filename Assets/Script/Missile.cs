using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	Vector3 direction;

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
	}

	void OnTriggerEnter(Collider other)
	{
		other.SendMessage("OnHit");
		DestroySelf ();
	}
}
