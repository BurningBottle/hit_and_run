using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIVersionText : MonoBehaviour
{
	void Start()
	{
		var builder = new StringBuilder().Append("Version : ")
			.Append(Application.version)
			.Append("\nUnity Version : ")
			.Append(Application.unityVersion)
			.Append("\nLanguage : ")
			.Append(Application.systemLanguage.ToString());

		GetComponent<Text>().text = builder.ToString();
	}
}
