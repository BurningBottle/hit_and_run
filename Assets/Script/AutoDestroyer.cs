using UnityEngine;
using System.Collections;

public class AutoDestroyer : MonoBehaviour
{
	public float delay;

	void Awake()
	{
		Invoke("SelfDestroy", delay);
	}

	void SelfDestroy()
	{
		GameObject.Destroy(gameObject);
	}
}
