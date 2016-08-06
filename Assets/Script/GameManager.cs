using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance = null;

	public GameObject playerA;
	public GameObject playerB;
	public GameObject missilePrefab;
	public GameObject virtualStick;
	public BlockManager[] blockManagers;

	[HideInInspector]
	public int myPlayerIndex = 0;

	MyPlayer myPlayer;
	RemotePlayer enemyPlayer;

	void Awake()
	{
		GameManager.instance = this;
		MyNetworkManager.instance.RegisterReceiveNotifier(PacketId.GameStart, OnStartGame);

		if(MyNetworkManager.instance.isServer)
		{
			myPlayerIndex = 0;
			myPlayer = playerA.AddComponent<MyPlayer>();
			enemyPlayer = playerB.AddComponent<RemotePlayer>();
		}
		else
		{
			myPlayerIndex = 1;
			myPlayer = playerB.AddComponent<MyPlayer>();
			enemyPlayer = playerA.AddComponent<RemotePlayer>();
		}
	}

	void Start()
	{
		if(!MyNetworkManager.instance.isServer)
		{
			var packetData = new LoadingCompleteData();
			packetData.playerId = myPlayerIndex;
			MyNetworkManager.instance.SendReliable(new LoadingCompletePacket(packetData));
		}
		else
		{
			MyNetworkManager.instance.OnGameSceneLoadComplete(null);
		}

		StartGame((int)System.DateTime.Now.Ticks);
	}

	void OnDestroy()
	{
		GameManager.instance = null;
	}

	public void ReadyToStartGame()
	{
		var packetData = new GameStartData();
		packetData.randomSeed = (int)System.DateTime.Now.Ticks;
		MyNetworkManager.instance.SendReliable(new GameStartPacket(packetData));

		StartGame(packetData.randomSeed);
	}

	void OnStartGame(byte[] data)
	{
		var packet = new GameStartPacket(data);
		StartGame(packet.GetPacket().randomSeed);
	}

	void StartGame(int randomSeed)
	{
		UnityEngine.Random.InitState(randomSeed);
		Invoke("GenerateBlocks", 0.5f);
	}

	void GenerateBlocks()
	{
		foreach(var blockManager in blockManagers)
			blockManager.Generate();

		virtualStick.SetActive(true);
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
		return myPlayer.transform.position;
	}

	public Vector3 GetEnemyPosition()
	{
		return enemyPlayer.transform.position;
	}
}
