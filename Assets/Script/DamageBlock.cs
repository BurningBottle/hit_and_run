using UnityEngine;
using System.Collections;

public class DamageBlock : MonoBehaviour 
{
	public GameObject root;
	public Transform marker;

	const float FALL_TIME = 1.0f;

	float elapsedFrame = 0.0f;
	Vector3 originMarkerScale;
	Vector3 originPosition;
	Vector3 destPosition;

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
		float t = Mathf.Min (1.0f, elapsedFrame / FALL_TIME);
		transform.position = Vector3.Lerp (originPosition, destPosition, t);
		marker.localScale = Vector3.Lerp (originMarkerScale, Vector3.one * 0.2f, t);
	}
		
//	void OnCollisionEnter(Collision collision)
//	{
//		Debug.Log ("OnCollisionEnter " + LayerMask.LayerToName (collision.collider.gameObject.layer));
//	}

	void OnTriggerEnter(Collider other)
	{
//		Debug.Log ("OnTriggerEnter " + LayerMask.LayerToName (other.gameObject.layer));
		var collidedLayer = LayerMask.LayerToName (other.gameObject.layer);
		if (string.Equals ("Terrain", collidedLayer))
			GameObject.Destroy (root);
//		else
//			Debug.Log ("OnTriggerEnter = Layer: " + collidedLayer);
	}
}
