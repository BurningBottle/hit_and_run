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
	public RadialBlur radialBlur;
	public GameObject hitFxPrefab;

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
		Time.timeScale = 1.0f;
		virtualStick.DisableJoystick ();
//		gameObject.AddComponent<MyNetworkManager> ();

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

		StartGameOverRoutine(winner, loser);
	}

	void OnReceiveGameOver(byte[] data)
	{
		var packetData = new GameOverPacket (data);
		var packet = packetData.GetPacket ();
		AbstractPlayerFsm winner = packet.myPlayerDead ? (AbstractPlayerFsm)enemyPlayer : (AbstractPlayerFsm)myPlayer;
		AbstractPlayerFsm loser = packet.myPlayerDead ? (AbstractPlayerFsm)myPlayer : (AbstractPlayerFsm)enemyPlayer;

		if (!packet.myPlayerDead)
			loser.transform.position = packet.position;

		StartGameOverRoutine(winner, loser);
	}

	void StartGameOverRoutine(AbstractPlayerFsm winner, AbstractPlayerFsm loser)
	{
		isGameOver = true;
		virtualStick.DisableJoystick();

		foreach (var blockManager in blockManagers)
			blockManager.StopGeneration();

		if (winner == myPlayer)
			StartCoroutine(VictoryRoutine(winner, loser));
		else
			StartCoroutine(LoseRoutine(winner, loser));
	}

	IEnumerator VictoryRoutine(AbstractPlayerFsm winner, AbstractPlayerFsm loser)
	{
		winner.GotoState(StateName.Wait);
		loser.GotoState(StateName.Die);

		yield return new WaitForSeconds (3.0f);

		winner.GotoState (StateName.Victory);

		var cameraPosition = winner.transform.position + winner.transform.forward * 3.0f;
		cameraPosition.y += 0.6f;
		var cameraRotation = Quaternion.LookRotation (winner.transform.forward * -1.0f);

		Time.timeScale = 0.1f;
		var originCameraPosition = Camera.main.transform.position;
		var originCameraRotation = Camera.main.transform.rotation;

		float elapsed = 0.0f;
		float t = 0.0f;
		while (elapsed < 1.7f) 
		{
			elapsed += Time.unscaledDeltaTime;
			t = elapsed / 1.7f;
			Camera.main.transform.position = Vector3.Lerp (originCameraPosition, cameraPosition, t * 1.5f);
			Camera.main.transform.rotation = Quaternion.Slerp (originCameraRotation, cameraRotation, t);

			yield return null;
		}

		yield return new WaitForSeconds (0.05f);

		Time.timeScale = 1.0f;
		GameUIManager.instance.ShowWinMessage (true);
	}

	IEnumerator LoseRoutine(AbstractPlayerFsm winner, AbstractPlayerFsm loser)
	{
		winner.GotoState(StateName.Wait);
		loser.GotoState(StateName.Die);

		var direction = (Camera.main.transform.position - loser.transform.position).normalized;
		var cameraPosition = loser.transform.position + direction * 4.0f;
		var originCameraPosition = Camera.main.transform.position;

		float elapsed = 0.0f;
		float t = 0.0f;
		while (elapsed < 1.7f) 
		{
			elapsed += Time.deltaTime;
			t = elapsed / 1.7f;
			Camera.main.transform.position = Vector3.Lerp (originCameraPosition, cameraPosition, t);

			yield return null;
		}

		yield return new WaitForSeconds(3.0f - 1.7f);

		winner.GotoState(StateName.Victory);
		GameUIManager.instance.ShowWinMessage(false);
	}

	public void SendRestartGame()
	{
		var packetData = new RestartGameData ();
		packetData.playerId = myPlayerIndex;
		MyNetworkManager.instance.SendReliable (new RestartGamePacket (packetData));

		MyNetworkManager.instance.RestartGame ();
	}

	public void CreateHitFx(Vector3 position)
	{
		GameObject.Instantiate(hitFxPrefab, position, Quaternion.identity);
	}
}
