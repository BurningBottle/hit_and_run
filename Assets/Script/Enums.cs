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
	public const float blockFallTime = 0.7f;
	public const float blockCreationInterval = 0.4f;
}