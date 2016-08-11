using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour 
{
	public static GameUIManager instance;

	public Text myHpText;
	public Text winMessageText;
	public Button restartButton;

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

	public void ShowWinMessage(bool winnerIsMe)
	{
		restartButton.gameObject.SetActive (MyNetworkManager.instance.isServer);
		winMessageText.gameObject.SetActive (true);

		if (winnerIsMe)
			winMessageText.text = "YOU WIN!!";
		else
			winMessageText.text = "YOU LOSE..";
	}
}
