using UnityEngine;

public class ActionManager : MonoBehaviour {

	[SerializeField]
	private GameObject boar;
	[SerializeField]
	private GameBoar boarGame;
	[SerializeField]
	private GameObject berry;
	[SerializeField]
	private GameBerry berryGame;

	private Data.actions currentGame;

	public void SetupAction(Data.actions action) {
		currentGame = action;
		DeactivateAll ();

		switch (action) {
			case Data.actions.boar:
				boar.SetActive (true);
				break;
			case Data.actions.berry:
				berry.SetActive (true);
				break;
		}
	}

	public void ActivateAction() {
		DeactivateAllGames();

		switch (currentGame) {
		case Data.actions.boar:
			boarGame.SetActive();
			break;
		case Data.actions.berry:
			berryGame.SetActive();
			break;
		}
	}

	private void DeactivateAll() {
		boar.SetActive (false);
		berry.SetActive (false);
		DeactivateAllGames ();
	}

	private void DeactivateAllGames() {
		boarGame.SetActive(false);
		berryGame.SetActive(false);
	}
}