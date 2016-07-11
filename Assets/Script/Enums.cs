using UnityEngine;
using System.Collections;

public enum StateName
{
	None,
	Wait,
	Run,
	Hit,
	Attack,
	Die,
	Victory
}

public static class MyConst
{
	public const float playerMoveSpeed = 3.0f;
}