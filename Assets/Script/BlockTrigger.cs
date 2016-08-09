using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockTrigger : MonoBehaviour 
{
	[HideInInspector]
	public bool isMine = true;

	void OnTriggerEnter(Collider other)
	{
		if(isMine)
			GameManager.instance.CreateMissileByMe(other.transform.position);

		other.SendMessage("OnMissileShoot");
	}
}
