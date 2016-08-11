using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractPlayerFsm : MonoBehaviour 
{
	public StateName currentStateName
	{
		get;
		private set;
	}

	public StateName prevStateName 
	{
		get;
		private set;
	}

	public int hp 
	{
		get;
		protected set;
	}

	private Animator animator 
	{
		get;
		set;
	}

	private NavMeshAgent navMeshAgent
	{
		get;
		set;
	}

//	StateName nextStateName = StateName.None;
	AbstractPlayerState currentState = null;
	Dictionary<StateName, AbstractPlayerState> stateMap = new Dictionary<StateName, AbstractPlayerState>();

	protected abstract void InitState();

	virtual protected void Awake()
	{
		animator = GetComponent<Animator> ();

		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.speed = MyConst.playerMoveSpeed;

		InitState ();
	}

	void Start()
	{
		SetHp (MyConst.playerMaxHp);
	}

	protected virtual void SetHp(int hp)
	{
		this.hp = Mathf.Max(hp, 0);
	}

	protected virtual void Damage()
	{
		SetHp (hp - 1);
	}

	protected void AddState(AbstractPlayerState state)
	{
		state.Init (this);
		stateMap.Add (state.name, state);
	}

	void Update()
	{
		currentState.Update ();
	}

//	void LateUpdate()
//	{
//		if (nextStateName != StateName.None)
//			TransiteState ();
//	}

	public void GotoState(StateName nextStateName)
	{
		var newState = GetState (nextStateName);
		if (newState == null) 
		{
			Debug.LogError ("[ERROR] State[" + nextStateName + "] is NOT EXIST!");
			return;
		}

		if (currentState != null) 
		{
			currentState.OnEnd ();
			prevStateName = currentState.name;
		}

		currentStateName = newState.name;

		newState.OnEnter ();
		currentState = newState;
	}

	public bool IsState(StateName stateName)
	{
		return currentStateName == stateName;
	}

//	public void GotoState(StateName stateName)
//	{
//		nextStateName = stateName;
//	}

	AbstractPlayerState GetState(StateName stateName)
	{
		if (!stateMap.ContainsKey (stateName))
			return null;

		return stateMap [stateName];
	}

	public void PlayAnimation(string animationName)
	{
		animator.Play (animationName, 0);
	}

	public void PlayFacialAnimation(string animationName)
	{
		animator.Play (animationName, 1);
	}

	public bool IsAnimationEnded()
	{
		var info = animator.GetCurrentAnimatorStateInfo (0);
		return info.normalizedTime >= info.length;
	}

	public AnimatorStateInfo GetCurrentAnimatorStateInfo()
	{
		return animator.GetCurrentAnimatorStateInfo (0);
	}

	void OnCallChangeFace(AnimationEvent animationEvent)
	{
		// for AnimationEvent
	}

	public void MoveByInputDirection(Vector2 inputDirection)
	{
		var moveDir = new Vector3(inputDirection.x, 0.0f, inputDirection.y);
		moveDir.Normalize();

		navMeshAgent.SetDestination(transform.position + (moveDir * MyConst.playerMoveSpeed));
	}

	public void StopMove()
	{
		navMeshAgent.Stop();
	}

	public void ResetPath()
	{
		navMeshAgent.ResetPath();
	}

	// By missiles or DamageBlocks, called by SendMessage.
	void OnHit()
	{
		if (IsState (StateName.Hit) || IsState(StateName.Die) || GameManager.instance.isGameOver)
			return;

		Damage ();

		if (hp == 0 && MyNetworkManager.instance.isServer)
			GameManager.instance.SendGameOver (this);
		else
			GotoState (StateName.Hit);
	}
}
