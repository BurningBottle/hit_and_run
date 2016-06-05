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

	private Animator animator 
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
		InitState ();
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

	void OnCallChangeFace(AnimationEvent animationEvent)
	{
		// for AnimationEvent
	}
}
