using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
	public GameObject mainButtons;
	public GameObject serverSideObjects;
	public GameObject clientSideObjects;
	public Text myIpText;
	public InputField ipInputField;

	public void OnClickStartServer()
	{
		mainButtons.SetActive(false);
		serverSideObjects.SetActive(true);
		myIpText.text = "IP : " + Network.player.ipAddress;

		MyNetworkManager.instance.StartServer();
    }

	public void OnClickJoin()
	{
		mainButtons.SetActive(false);
		clientSideObjects.SetActive(true);
	}

	public void OnClickConfirmJoin()
	{
		MyNetworkManager.instance.Connect(ipInputField.text);
	}
}
