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
	public const int playerMaxHp = 10;

	public const float blockFallTime = 0.8f;
	public const float blockCreationInterval = 0.45f;

	public const float missileBlockLife = 3.0f;
	public const int missileBlockRate = 2000;

	public const float missileVelocity = 8.0f;
	public const float missileLife = 10.0f;
}