using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour 
{
	public static GameUIManager instance;

	public Text myHpText;

	void Awake()
	{
		GameUIManager.instance = this;
	}

	void OnDestroy()
	{
		GameUIManager.instance = null;
	}

	public void SetHp(int hp)
	{
		myHpText.text = "HP : " + hp;
	}
}
