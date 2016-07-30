using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour 
{
	public Transform baseTransform;
	public GameObject blockPrefab;

	const int N = 16;
	const float INTERVAL = 1.2f;

	Coroutine generateRoutine = null;
	float unitX;
	float unitZ;
	float previousIndexX = -1.0f;
	float previousIndexZ = -1.0f;

	void Start () 
	{
		transform.position = baseTransform.position + new Vector3 (0.0f, baseTransform.localScale.y * 0.5f, 0.0f);
		unitX = baseTransform.localScale.x / (float)N;
		unitZ = baseTransform.localScale.z / (float)N;

		Generate ();	
	}

	public void Generate()
	{
		generateRoutine = StartCoroutine (GenerateRoutine ());
	}

	IEnumerator GenerateRoutine()
	{
		float indexX, indexZ;

		while (true) 
		{
			do 
			{
				indexX = UnityEngine.Random.Range ((-N / 2) + 1, (N / 2) - 1);
				indexZ = UnityEngine.Random.Range ((-N / 2) + 1, (N / 2) - 1);
			} while(indexX == previousIndexX && indexZ == previousIndexZ);

			Vector3 blockPosition = new Vector3 (transform.position.x + unitX * indexX, transform.position.y + 0.1f, transform.position.z + unitZ * indexZ);
			GameObject.Instantiate (blockPrefab, blockPosition, Quaternion.identity);

			//Debug.Log ("Block Index (" + indexX + "," + indexZ + ")");

			previousIndexX = indexX;
			previousIndexZ = indexZ;

			yield return new WaitForSeconds (INTERVAL);
		}
	}

	public void StopGeneration()
	{
		if (generateRoutine != null) 
		{
			StopCoroutine (generateRoutine);
			generateRoutine = null;
		}
	}
}
