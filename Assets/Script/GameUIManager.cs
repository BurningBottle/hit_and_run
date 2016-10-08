using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour 
{
	public static GameUIManager instance;

	public Text myHpText;
	public Button restartButton;
	public Image victoryImage;
	public Image loseImage;

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

		if (winnerIsMe)
			victoryImage.gameObject.SetActive(true);
		else
			loseImage.gameObject.SetActive(true);
	}
}
