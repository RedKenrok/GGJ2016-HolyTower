using UnityEngine;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour {

	[SerializeField]
	private Image displayContainer;

	public void SetDisplayContainer(string imagePath) {
		if (imagePath != "") {
			displayContainer.sprite = Resources.Load<Sprite>("Sprites/Display/" + imagePath);
		}
	}
}