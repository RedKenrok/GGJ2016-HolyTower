using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private GameObject playLarge;
	[SerializeField]
	private GameObject playSmall;
	[SerializeField]
	private GameObject quitLarge;
	[SerializeField]
	private GameObject quiteSmall;

	private bool selectedStart = true;

	private void Update() {
		PlayerInput playerInput = InputHandler.GetInput();

		if (!selectedStart && playerInput.horizontalKeyDown && playerInput.horizontal < 0) {
			selectedStart = true;
			playLarge.SetActive(true);
			playSmall.SetActive(false);
			quitLarge.SetActive(false);
			quiteSmall.SetActive(true);
		}
		else if (selectedStart && playerInput.horizontalKeyDown && playerInput.horizontal > 0) {
			selectedStart = false;
			playLarge.SetActive(false);
			playSmall.SetActive(true);
			quitLarge.SetActive(true);
			quiteSmall.SetActive(false);
		}

		if (playerInput.acceptKeyDown) {
			if (selectedStart) {
				SceneManager.LoadScene("Game");
			}
			else {
				Application.Quit();
			}
		}
	}
}