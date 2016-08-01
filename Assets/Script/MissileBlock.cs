using UnityEngine;
using System.Collections;

public class MissileBlock : MonoBehaviour 
{
	public GameObject root;
	public Transform marker;

	float elapsedFrame = 0.0f;
	Vector3 originMarkerScale;
	Vector3 originPosition;
	Vector3 destPosition;
	bool isFalling = true;
	BlockTrigger connectedTrigger;

	void Start () 
	{
		originPosition = transform.position;
		destPosition = marker.position;
		destPosition.y -= 0.2f;
		originMarkerScale = marker.localScale;
	}

	void Update () 
	{
		elapsedFrame += Time.deltaTime;	

		if (isFalling) 
		{
			float t = Mathf.Min (1.0f, elapsedFrame / MyConst.blockFallTime);
			transform.position = Vector3.Lerp (originPosition, destPosition, t);
			marker.localScale = Vector3.Lerp (originMarkerScale, Vector3.one * 0.2f, t);
		} 
		else 
		{
			if (elapsedFrame >= MyConst.missileBlockLife) 
			{
				if (connectedTrigger != null)
					connectedTrigger.RemoveBlock (GetComponent<BoxCollider>());

				GameObject.Destroy (root);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isFalling)
			return;
		
		var collidedLayer = LayerMask.LayerToName (other.gameObject.layer);
		if (string.Equals ("Terrain", collidedLayer)) 
		{
			isFalling = false;
			elapsedFrame = 0.0f;
			GetComponent<MeshRenderer> ().sharedMaterial.color = Color.blue;
		}
	}

	void OnBlockTriggerEnter(BlockTrigger trigger)
	{
		connectedTrigger = trigger;
	}

	void OnBlockTriggerExit(BlockTrigger trigger)
	{
		connectedTrigger = null;
	}

	void OnMissileShoot()
	{
		GameObject.Destroy (root);
	}
}
