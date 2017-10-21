using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour {

	[SerializeField]
	private GameObject prefabOption;

	[SerializeField]
	private GameObject optionsGuardians;
	[SerializeField]
	private GameObject optionsOffer;

	[SerializeField]
	private GameObject selector;

	private bool selectingGuardian = true;
	private Data.guardians selectionGuardian = Data.guardians.weather;
	private Data.offers selectionOffer = Data.offers.boar;

	private void Awake() {
		for (int i = 0; i < Data.guardiansCount; i++) {
			GameObject option = Instantiate(prefabOption);
			option.transform.SetParent(optionsGuardians.transform, false);
			option.GetComponent<RectTransform>().localPosition = new Vector3(Data.guardiansOptionWidth, Data.guardiansOptionRange[0] + ((Data.guardiansOptionRange[1] - Data.guardiansOptionRange[0]) / (Data.guardiansCount - 1) * i));
			option.transform.FindChild("TextField").GetComponent<Text>().text = Data.guardiansText[i];
		}

		for (int i = 0; i < Data.offersCount; i++) {
			GameObject option = Instantiate(prefabOption);
			option.transform.SetParent(optionsOffer.transform, false);
			option.GetComponent<RectTransform>().localPosition = new Vector3(Data.offersOptionWidth, Data.offersOptionRange[0] + ((Data.offersOptionRange[1] - Data.offersOptionRange[0]) / (Data.offersCount - 1) * i));
			option.transform.FindChild("TextField").GetComponent<Text>().text = Data.offersText[i];
		}
	}

	private void Start() {
		Reset();
	}

	public void Reset() {
		Debug.Log ("Resetting");
		selectingGuardian = true;
		selectionGuardian = Data.guardians.weather;
		selectionOffer = Data.offers.boar;
		selector.GetComponent<RectTransform> ().localPosition = new Vector3 (Data.guardiansOptionWidth - 256f, Data.guardiansOptionRange [0] + ((Data.guardiansOptionRange [1] - Data.guardiansOptionRange [0]) / (Data.guardiansCount - 1) * ((int)selectionGuardian)));
	}

	private void Update() {
		if (!GameHandler.gameHandler.textManager.isPrinting) {
			PlayerInput playerInput = InputHandler.GetInput();

			if (selectingGuardian) {
				if (playerInput.verticalKeyDown) {
					selectionGuardian = (Data.guardians)Mathf.Clamp (((int)selectionGuardian - (int)playerInput.vertical), 0, Data.guardiansCount - 1);
				}
				if (playerInput.acceptKeyDown) {
					selectingGuardian = false;
				}
				selector.GetComponent<RectTransform> ().localPosition = new Vector3 (Data.guardiansOptionWidth - 256f, Data.guardiansOptionRange [0] + ((Data.guardiansOptionRange [1] - Data.guardiansOptionRange [0]) / (Data.guardiansCount - 1) * ((int)selectionGuardian)));
			} else {
				if (playerInput.verticalKeyDown) {
					selectionOffer = (Data.offers)Mathf.Clamp (((int)selectionOffer - (int)playerInput.vertical), 0, Data.offersCount - 1);
				}
				if (playerInput.declineKeyDown) {
					selectingGuardian = true;
				}
				if (playerInput.acceptKeyDown) {
					GameHandler.gameHandler.playAction (selectionGuardian, selectionOffer);
				}
				selector.GetComponent<RectTransform> ().localPosition = new Vector3 (Data.offersOptionWidth - 256f, Data.offersOptionRange [0] + ((Data.offersOptionRange [1] - Data.offersOptionRange [0]) / (Data.offersCount - 1) * ((int)selectionOffer)));
			}
		}
	}
}