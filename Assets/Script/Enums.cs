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
	public const int playerMaxHp = 5;

	public const float blockFallTime = 0.7f;
	public const float blockCreationInterval = 0.4f;

	public const float missileBlockLife = 3.0f;
	public const int missileBlockRate = 2000;

	public const float missileVelocity = 12.0f;
	public const float missileLife = 10.0f;
}