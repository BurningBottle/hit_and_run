using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour 
{
	public Transform baseTransform;
	public GameObject damageBlockPrefab;
	public GameObject missileBlockPrefab;

	const int N = 16;

	Coroutine generateRoutine = null;
	float unitX;
	float unitZ;
	float previousIndexX = -1.0f;
	float previousIndexZ = -1.0f;
	bool generation = true;

	void Start () 
	{
		transform.position = baseTransform.position + new Vector3 (0.0f, baseTransform.localScale.y * 0.5f, 0.0f);
		unitX = baseTransform.localScale.x / (float)N;
		unitZ = baseTransform.localScale.z / (float)N;
	}

	public void Generate()
	{
		generation = true;
		generateRoutine = StartCoroutine (GenerateRoutine ());
	}

	IEnumerator GenerateRoutine()
	{
		float indexX, indexZ;

		while (generation) 
		{
			do 
			{
				indexX = UnityEngine.Random.Range ((-N / 2) + 1, (N / 2) - 1);
				indexZ = UnityEngine.Random.Range ((-N / 2) + 1, (N / 2) - 1);
			} while(indexX == previousIndexX && indexZ == previousIndexZ);

			Vector3 blockPosition = new Vector3 (transform.position.x + unitX * indexX, transform.position.y + 0.1f, transform.position.z + unitZ * indexZ);

			GameObject blockPrefab = null;
			if (UnityEngine.Random.Range (0, 10000) <= MyConst.missileBlockRate)
				blockPrefab = missileBlockPrefab;
			else
				blockPrefab = damageBlockPrefab;
			
			GameObject.Instantiate (blockPrefab, blockPosition, Quaternion.identity);

			//Debug.Log ("Block Index (" + indexX + "," + indexZ + ")");

			previousIndexX = indexX;
			previousIndexZ = indexZ;

			yield return new WaitForSeconds (MyConst.blockCreationInterval);
		}
	}

	public void StopGeneration()
	{
		generation = false;
		generateRoutine = null;
//		if (generateRoutine != null) 
//		{
//			StopCoroutine (generateRoutine);
//			StopCoroutine(GenerateRoutine());
//			generateRoutine = null;
//		} 
	}
}
