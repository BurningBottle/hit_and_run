using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance = null;

	public GameObject playerA;
	public GameObject playerB;
	public GameObject missilePrefab;
	public VirtualJoystickRegion virtualStick;
	public BlockManager[] blockManagers;

	[HideInInspector]
	public int myPlayerIndex = 0;

	public bool isGameOver 
	{
		get;
		private set;
	}

	MyPlayer myPlayer;
	RemotePlayer enemyPlayer;

	void Awake()
	{
		GameManager.instance = this;
		isGameOver = false;
		virtualStick.DisableJoystick ();

		MyNetworkManager.instance.RegisterReceiveNotifier(PacketId.GameStart, OnStartGame);
		MyNetworkManager.instance.RegisterReceiveNotifier(PacketId.ShotMissile, OnReceiveShotMissile);
		MyNetworkManager.instance.RegisterReceiveNotifier (PacketId.GameOver, OnReceiveGameOver);

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

		virtualStick.EnableJoystick ();
	}

	public void CreateMissileByMe(Vector3 start)
	{
		Vector3 dest = GetEnemyPosition ();
		dest.y += 0.5f;
		start.y = dest.y;

		var packetData = new ShotMissileData();
		packetData.from = start;
		packetData.dest = dest;
		MyNetworkManager.instance.SendReliable(new ShotMissilePacket(packetData));

		var missileObject = (GameObject)GameObject.Instantiate (missilePrefab, start, Quaternion.identity);
		var layerName = "Missile";
		missileObject.layer = LayerMask.NameToLayer(layerName);

		var missile = missileObject.GetComponent<Missile> ();
		missile.Shoot (start, dest);
	}

	void OnReceiveShotMissile(byte[] data)
	{
		var packetData = new ShotMissilePacket(data);
		var packet = packetData.GetPacket();
		CreateMissileByEnemy(packet.from, packet.dest);
	}

	public void CreateMissileByEnemy(Vector3 start, Vector3 dest)
	{
		var missileObject = (GameObject)GameObject.Instantiate(missilePrefab, start, Quaternion.identity);
		var layerName = "EnemyMissile";
		missileObject.layer = LayerMask.NameToLayer(layerName);

		var missile = missileObject.GetComponent<Missile>();
		missile.Shoot(start, dest);
	}

	public Vector3 GetMyPosition()
	{
		return myPlayer.transform.position;
	}

	public Vector3 GetEnemyPosition()
	{
		return enemyPlayer.transform.position;
	}

	public void SendGameOver(AbstractPlayerFsm deadPlayer)
	{
		bool myPlayerDead = (deadPlayer == myPlayer);
		AbstractPlayerFsm winner = myPlayerDead ? (AbstractPlayerFsm)enemyPlayer : (AbstractPlayerFsm)myPlayer;
		AbstractPlayerFsm loser = myPlayerDead ? (AbstractPlayerFsm)myPlayer : (AbstractPlayerFsm)enemyPlayer;

		var packetData = new GameOverData ();
		packetData.myPlayerDead = !myPlayerDead;
		packetData.position = deadPlayer.transform.position;
		MyNetworkManager.instance.SendReliable (new GameOverPacket (packetData));

		StartCoroutine (GameOverRoutine (winner, loser));
	}

	void OnReceiveGameOver(byte[] data)
	{
		var packetData = new GameOverPacket (data);
		var packet = packetData.GetPacket ();
		AbstractPlayerFsm winner = packet.myPlayerDead ? (AbstractPlayerFsm)enemyPlayer : (AbstractPlayerFsm)myPlayer;
		AbstractPlayerFsm loser = packet.myPlayerDead ? (AbstractPlayerFsm)myPlayer : (AbstractPlayerFsm)enemyPlayer;

		if (!packet.myPlayerDead)
			loser.transform.position = packet.position;

		StartCoroutine (GameOverRoutine (winner, loser));
	}

	IEnumerator GameOverRoutine(AbstractPlayerFsm winner, AbstractPlayerFsm loser)
	{
		isGameOver = true;
		virtualStick.DisableJoystick ();

		foreach (var blockManager in blockManagers)
			blockManager.StopGeneration ();

		winner.GotoState (StateName.Wait);
		loser.GotoState (StateName.Die);

		yield return new WaitForSeconds (2.5f);

		winner.GotoState (StateName.Victory);
	}
}
