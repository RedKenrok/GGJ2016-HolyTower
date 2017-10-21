using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {

	public static GameHandler gameHandler;

	public TextManager textManager;
	public DisplayManager displayManager;
	public ChoiceManager choiceManager;
	public ActionManager actionManager;

	private List<Data.events> eventDeck = new List<Data.events>();
	private int shuffleCount = 64;

	private TableEvent tableEvent;
	private Data.guardians guardian;
	private Data.offers offer;

	private void Awake() {
		Data.Reset();
		gameHandler = this;
		NewEventDeck();
		StartCoroutine(PlayIntro());
	}

	private bool NewEventDeck() {
		if (eventDeck.Count <= 0) {
			eventDeck = new List<Data.events>();
			for (int i = 0; i < Data.eventsCount; i++) {
				eventDeck.Add ((Data.events)i);
			}

			for (int i = 0; i < shuffleCount; i++) {
				int[] randomNumbers = new int[2] { Random.Range (0, Data.eventsCount -1), Random.Range (0, Data.eventsCount -1) };
				Data.events eventTemp = eventDeck[randomNumbers[0]];
				eventDeck[randomNumbers[0]] = eventDeck [randomNumbers[1]];
				eventDeck[randomNumbers[1]] = eventTemp;
			}

			return true;
		}
		return false;
	}

	private IEnumerator PlayIntro() {
		TableIntro table = DatabaseHandler.GetFromDatabaseIntro(Data.intros.intro);
		displayManager.SetDisplayContainer(table.imagePath);
		textManager.SetTextArray(table.text);
		textManager.Next();

		while (true) {
			if (InputHandler.GetInput().accept) {
				if (textManager.textLength <= 0) {
					break;
				}
				else {
					textManager.Next();
				}
			}
			yield return null;
		}

		while (textManager.isPrinting || !InputHandler.GetInput().acceptKeyDown) {
			yield return null;
		}

		Data.events eventCur = eventDeck[0];
		eventDeck.RemoveAt(0);
		StartCoroutine(PlayEvent(eventCur));
	}

	private IEnumerator PlayEvent(Data.events newEvent) {
		tableEvent = DatabaseHandler.GetFromDatabaseEvents(newEvent);
		displayManager.SetDisplayContainer(tableEvent.intro.imagePath);
		textManager.SetTextArray(tableEvent.intro.text);
		textManager.Next();

		while (true) {
			if (InputHandler.GetInput().accept) {
				if (textManager.textLength <= 0) {
					break;
				}
				else {
					textManager.Next();
				}
			}
			yield return null;
		}

		while (textManager.isPrinting || !InputHandler.GetInput().acceptKeyDown) {
			yield return null;
		}

		textManager.SetTextArray(new string[1] { "To which guardian would you like to make the offering?" });
		textManager.Next();

		displayManager.gameObject.SetActive(false);
		choiceManager.gameObject.SetActive(true);
		choiceManager.Reset();
	}

	public void playAction(Data.guardians _guardian, Data.offers _offer) {
		StartCoroutine (PlayAction(_guardian, _offer));
	}

	private IEnumerator PlayAction(Data.guardians _guardian, Data.offers _offer) {
		guardian = _guardian;
		offer = _offer;

		choiceManager.gameObject.SetActive(false);
		actionManager.gameObject.SetActive(true);
		actionManager.SetupAction((Data.actions)offer);

		TableActions table = DatabaseHandler.GetFromDatabaseActions((Data.actions) offer);
		textManager.SetTextArray(table.rows[0].intro);
		textManager.Next();

		while (true) {
			if (InputHandler.GetInput().accept) {
				if (textManager.textLength <= 0) {
					break;
				}
				else {
					textManager.Next();
				}
			}
			yield return null;
		}

		while (textManager.isPrinting || !InputHandler.GetInput().acceptKeyDown) {
			yield return null;
		}

		actionManager.ActivateAction();
	}

	public void actionComplete(Data.succesRate succesRate) {
		StartCoroutine(ActionComplete(succesRate));
	}

	private IEnumerator ActionComplete(Data.succesRate succesRate) {
		Data.actionsTaken++;
		if (offer == Data.offers.boar) {
			Data.killsCommited++;
		}

		TableActions table = DatabaseHandler.GetFromDatabaseActions((Data.actions) offer);
		int randomNumber = Random.Range (0, table.rows.Length - 1);
		switch (succesRate) {
			case Data.succesRate.positive:
				textManager.SetTextArray (table.rows [randomNumber].positive);
				Data.towerStage++;
				break;
			case Data.succesRate.neutral:
				textManager.SetTextArray(table.rows[randomNumber].neutral);
				break;
			case Data.succesRate.negative:
				textManager.SetTextArray (table.rows [randomNumber].negative);
				Data.destroyedStructures++;
				break;
		}
		textManager.Next();

		while (true) {
			if (InputHandler.GetInput().accept) {
				if (textManager.textLength <= 0) {
					break;
				}
				else {
					textManager.Next();
				}
			}
			yield return null;
		}

		while (textManager.isPrinting || !InputHandler.GetInput().acceptKeyDown) {
			yield return null;
		}

		actionManager.gameObject.SetActive(false);
		RowEvent row = tableEvent.result((int)guardian, (int)succesRate);
		displayManager.gameObject.SetActive (true);
		displayManager.SetDisplayContainer(row.imagePath);
		textManager.SetTextArray(row.text);
		textManager.Next();

		while (true) {
			if (InputHandler.GetInput().accept) {
				if (textManager.textLength <= 0) {
					break;
				}
				else {
					textManager.Next();
				}
			}
			yield return null;
		}

		while (textManager.isPrinting || !InputHandler.GetInput().acceptKeyDown) {
			yield return null;
		}

		if (Data.towerStage >= Data.winningCount) {

			if (Data.killsCommited / Data.actionsTaken < Data.neutralThreshold) {
				StartCoroutine (PlayEpilogue (Data.succesRate.positive));
			}
			else {
				StartCoroutine(PlayEpilogue(Data.succesRate.neutral));
			}
		}
		else if (Data.destroyedStructures >= Data.losingCount) {
			StartCoroutine(PlayEpilogue(Data.succesRate.negative));
		}
		else if (row.nextEvent != -1) {
			tableEvent = DatabaseHandler.GetFromDatabaseEvents((Data.events)row.nextEvent);

			textManager.SetTextArray(new string[1] { "To which guardian would you like to make the offering?" });
			textManager.Next();

			displayManager.gameObject.SetActive(false);
			choiceManager.gameObject.SetActive(true);
			choiceManager.Reset();
		}
		else {
			NewEventDeck();
			Data.events eventCur = eventDeck[0];
			eventDeck.RemoveAt(0);
			StartCoroutine(PlayEvent(eventCur));
		}
	}

	private IEnumerator PlayEpilogue(Data.succesRate succes) {
		TableIntro table = new TableIntro();
		switch (succes) {
			case Data.succesRate.positive:
				table = DatabaseHandler.GetFromDatabaseIntro (Data.intros.epiloguePositive);
				break;
			case Data.succesRate.neutral:
				table = DatabaseHandler.GetFromDatabaseIntro (Data.intros.epilogueNeutral);
				break;
			case Data.succesRate.negative:
				table = DatabaseHandler.GetFromDatabaseIntro (Data.intros.epilogueNegative);
				break;
		}
		displayManager.SetDisplayContainer(table.imagePath);
		textManager.SetTextArray(table.text);
		textManager.Next();

		while (true) {
			if (InputHandler.GetInput().accept) {
				if (textManager.textLength <= 0) {
					break;
				}
				else {
					textManager.Next();
				}
			}
			yield return null;
		}

		while (textManager.isPrinting || !InputHandler.GetInput().acceptKeyDown) {
			yield return null;
		}

		SceneManager.LoadScene("Menu");
	}
}