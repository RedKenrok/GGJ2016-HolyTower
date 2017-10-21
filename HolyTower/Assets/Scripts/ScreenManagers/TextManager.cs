using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

	private List<string> textList = new List<string>();
	public int textLength {
		get {
			return textList.Count;
		}
	}

    [SerializeField]
    private Text textField;

	[SerializeField]
	private GameObject nextIndicator;

	private bool IsPrinting = false;
	public bool isPrinting {
		get {
			return IsPrinting;
		}
	}

	public bool SetTextArray(string[] textArray) {
		if (textList.Count <= 0) {
			textList = new List<string>();
			for (int i = 0; i < textArray.Length; i++) {
				textList.Add(textArray[i]);
			}
			return true;
		}
		return false;
	}
    
    public bool Next() {
		if (!isPrinting && textLength > 0) {
			StartCoroutine(ScrollText(textField, textList[0]));
			textList.RemoveAt(0);

			return true;
		}
        return false;
	}

	private IEnumerator ScrollText(Text field, string text) {
		IsPrinting = true;
		field.text = "";
		for (int i = 0; i < text.Length; i++) {
			field.text += text[i];
			if (InputHandler.GetInput ().accept) {
				yield return new WaitForSeconds (0.01f);
			}
			else {
				yield return new WaitForSeconds (0.05f);
			}
		}
		IsPrinting = false;
	}

	private void Awake() {
		StartCoroutine(BlinkNextIndicator());
	}

	private IEnumerator BlinkNextIndicator() {
		while(true) {
			if (isPrinting) {
				nextIndicator.SetActive(!nextIndicator.activeSelf);
			} else {
				nextIndicator.SetActive(true);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}