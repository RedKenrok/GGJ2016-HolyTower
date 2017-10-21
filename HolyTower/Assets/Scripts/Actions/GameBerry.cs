using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBerry : MonoBehaviour {

	private bool active = false;
	public void SetActive(bool _active =true) {
		Reset();
		active = _active;
	}

	private static readonly int[] berryThreshold = new int[2] { 4, 8 };
	private int berriesGathered = 0;

	private bool spawningBerries = false;

	[SerializeField]
	private GameObject prefabBerry;
	private List<GameObject> berries = new List<GameObject>();
	private float spawnRate = 2f;
	private float scaleFactor = 0.99f;
	private float sizeMin = 0.1f;
	private float berryRadius = 72f;

	[SerializeField]
	private GameObject player;
	private RectTransform playerTransform;
	private Vector3 playerPositionDefault = new Vector3(0f, -75f, 0f);
	private float playerMovement = 256f;
	private float playerRadius = 72f;

	private float timeCurrent = 0f;
	private float timeDuration = 30f;

	private Vector3[] spawnRange = new Vector3[2] { new Vector3(-720f, -360f, 0f), new Vector3(720f, 84f, 0f)};

	private void Awake() {
		Reset ();
	}

	private void Reset() {
		playerTransform = player.GetComponent<RectTransform>();
		berriesGathered = 0;
		spawningBerries = false;
		playerTransform.localPosition = playerPositionDefault;
		timeCurrent = 0f;
		for (int i = 0; i < berries.Count; i++) {
			GameObject berry = berries [i];
			berries.Remove (berry);
			Destroy (berry);
		}
	}

	private void Update() {
		if (active) {
			if (!spawningBerries) {
				spawningBerries = true;
				StartCoroutine(SpawnBerries());
			}

			PlayerInput playerInput = InputHandler.GetInput();
			playerTransform.localPosition += (new Vector3 (playerInput.horizontal, playerInput.vertical, 0f)).normalized * playerMovement * Time.deltaTime;

			for (int i = 0; i < berries.Count; i++) {
				if (Vector3.Distance(berries[i].GetComponent<RectTransform>().localPosition, playerTransform.localPosition) < (berryRadius * berries[i].GetComponent<RectTransform>().localScale.x) + playerRadius) {
					GameObject berry = berries [i];
					berries.Remove (berry);
					berriesGathered++;
					Destroy (berry);
				}
			}

			timeCurrent += Time.deltaTime;
			if (timeCurrent > timeDuration) {
				active = false;
				if (berriesGathered > berryThreshold[1]) {
					GameHandler.gameHandler.actionComplete(Data.succesRate.positive);
					active = false;
				}
				else if (berriesGathered > berryThreshold[0]) {
					GameHandler.gameHandler.actionComplete(Data.succesRate.neutral);
					active = false;
				}
				else {
					GameHandler.gameHandler.actionComplete(Data.succesRate.negative);
					active = false;
				}
			}

			for (int i = 0; i < berries.Count; i++) {
				berries [i].GetComponent<RectTransform> ().localScale *= scaleFactor;
				if (berries [i].GetComponent<RectTransform> ().localScale.x < (prefabBerry.GetComponent<RectTransform> ().localScale * sizeMin).x) {
					GameObject berry = berries [i];
					berries.Remove (berry);
					Destroy (berry);
				}
			}
		}
	}

	private IEnumerator SpawnBerries() {
		while (active) {
			yield return new WaitForSeconds (spawnRate);
			GameObject berry = (GameObject)Instantiate (prefabBerry, Vector3.zero, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
			berry.transform.SetParent (transform, false);
			berry.GetComponent<RectTransform> ().localPosition = new Vector3(Random.Range(spawnRange[0].x, spawnRange[1].x), Random.Range(spawnRange[0].y, spawnRange[1].y), 0f);
			berries.Add (berry);
		}
	}
}