using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogWriter : MonoBehaviour {

	[SerializeField] float letterPause = 0.1f;
	string message;
	Text textComp;

	void Start () {
		textComp = GetComponent<Text>();
		message = textComp.text;
		textComp.text = "";
		StartCoroutine(TypeText ());
	}

	IEnumerator TypeText () {
		foreach (char letter in message.ToCharArray()) 
		{
			textComp.text += letter;
			yield return new WaitForSeconds (letterPause);
		}
	}
}