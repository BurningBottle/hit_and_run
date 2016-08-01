using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockTrigger : MonoBehaviour 
{
	List<Collider> blockList = new List<Collider>();

	void OnTriggerEnter(Collider other)
	{
		blockList.Add (other);
		other.SendMessage ("OnBlockTriggerEnter", this);
	}

	void OnTriggerExit(Collider other)
	{
		blockList.Remove (other);
		other.SendMessage ("OnBlockTriggerEnter", this);
	}

	public void RemoveBlock(Collider removed)
	{
		blockList.Remove (removed);
	}

	void Update()
	{
		if (Input.GetMouseButton (0) && blockList.Count > 0) 
		{
			foreach (var block in blockList) 
			{
				GameManager.instance.CreateMissile (block.transform.position, true);
				block.SendMessage ("OnMissileShoot");
			}

			blockList.Clear ();
		}
	}
}
