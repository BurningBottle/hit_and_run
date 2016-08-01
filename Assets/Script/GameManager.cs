using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance = null;

	public GameObject playerA;
	public GameObject playerB;
	public GameObject missilePrefab;

	[HideInInspector]
	public int myPlayerIndex = 0;

	void Awake()
	{
		GameManager.instance = this;
	}

	void OnDestroy()
	{
		GameManager.instance = null;
	}

	public void CreateMissile(Vector3 start, bool isMyMissile)
	{
		Vector3 dest = isMyMissile ? GetEnemyPosition () : GetMyPosition ();
		dest.y += 0.5f;
		start.y = dest.y;

		var missileObject = (GameObject)GameObject.Instantiate (missilePrefab, start, Quaternion.identity); 
		var layerName = isMyMissile ? "Missile" : "EnemyMissile";
		missileObject.layer = LayerMask.NameToLayer(layerName);

		var missile = missileObject.GetComponent<Missile> ();
		missile.Shoot (start, dest);
	}

	public Vector3 GetMyPosition()
	{
		return (myPlayerIndex == 0) ? playerA.transform.position : playerB.transform.position;
	}

	public Vector3 GetEnemyPosition()
	{
		return (myPlayerIndex == 0) ? playerB.transform.position : playerA.transform.position;
	}
}
