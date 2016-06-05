using UnityEngine;
using System.Collections;

public abstract class AbstractPlayerState 
{
	public StateName name 
	{
		get;
		protected set;
	}

	protected AbstractPlayerFsm player 
	{
		get;
		private set;
	}

	public void Init(AbstractPlayerFsm player)
	{
		this.player = player;
		InitName ();
	}

	protected abstract void InitName();

	public virtual void OnEnter()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnEnd()
	{
	}
}
