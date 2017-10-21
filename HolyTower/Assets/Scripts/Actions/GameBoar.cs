using UnityEngine;
using UnityEngine.UI;

public class GameBoar : MonoBehaviour {

	private bool active = false;
	public void SetActive(bool _active =true) {
		Reset();
		active = _active;
	}

	[SerializeField]
	private GameObject heart;
	private RectTransform heartTransform;
	private Vector3 heartOffset = new Vector3(-32f, -12f, 0f);
	private float heartRadius = 24;
	[SerializeField]
	private GameObject boar;
	private RectTransform boarTransform;
	private Vector3 boarPositionDefault = new Vector3 (932f, -120f, 0f);
	private float boarRadius = 64;
	[SerializeField]
	private GameObject player;
	private RectTransform playerTransform;
	private Vector3 playerPositionDefault = new Vector3 (0f, -400f, 0f);
	private float playerRadius = 42;

	private float[] rotationRange = new float[2] { -60f, 60f };
	private float currentRotation = 0f;
	private float rotationSpeed = 32f;

	private Vector3 targetPosition = Vector3.zero;
	private float distanceThreshold = 8f;

	private Vector3[] positionRange = new Vector3[2] { new Vector3(-680f, -192f, 0f), new Vector3(680f, 72f, 0f)};

	private float boarSpeed = 256f;

	private bool spearFired = false;
	private float spearSpeed = 16f;

	private bool hitBoar = false;

	private void Awake() {
		Reset();
	}

	private void Reset() {
		heartTransform = heart.GetComponent<RectTransform>();
		boarTransform = boar.GetComponent<RectTransform>();
		playerTransform = player.GetComponent<RectTransform>();
		boarTransform.localPosition = new Vector3((Random.Range(0, 1) == 0) ? boarPositionDefault.x : -boarPositionDefault.x, boarPositionDefault.y, boarPositionDefault.z);
		playerTransform.localPosition = playerPositionDefault;
		playerTransform.localRotation = Quaternion.identity;
		currentRotation = 0f;
		spearFired = false;
		hitBoar = false;
	}

	private void Update() {
		if (active) {
			PlayerInput playerInput = InputHandler.GetInput();

			float angleDelta = (-playerInput.horizontal) * rotationSpeed * Time.deltaTime;
			currentRotation = Mathf.Clamp (currentRotation + angleDelta, rotationRange [0], rotationRange [1]);
			if (angleDelta != 0) {
					playerTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, currentRotation));
			}

			if (playerInput.acceptKeyDown) {
				spearFired = true;
			}

			if (spearFired) {
				player.transform.Translate (transform.up * spearSpeed * Time.deltaTime);
			}

			if (Vector3.Distance(boarTransform.localPosition, targetPosition) < distanceThreshold || targetPosition == Vector3.zero) {
				targetPosition = new Vector3(Random.Range(positionRange[0].x, positionRange[1].x), Random.Range(positionRange[0].y, positionRange[1].y), 0f);
			}
			boarTransform.localPosition = Vector3.MoveTowards(boarTransform.localPosition, targetPosition, boarSpeed * Time.deltaTime);
			heartTransform.localPosition = heartOffset;

			if (Vector3.Distance(boarTransform.localPosition + heartTransform.localPosition, playerTransform.localPosition + (playerTransform.up * 2)) < heartRadius + playerRadius) {
				GameHandler.gameHandler.actionComplete(Data.succesRate.positive);
				active = false;
			}
			else if (Vector3.Distance(boarTransform.localPosition, playerTransform.localPosition + (playerTransform.up * 2)) < boarRadius + playerRadius) {
				hitBoar = true;
			}

			if (hitBoar && !(Vector3.Distance(boarTransform.localPosition, playerTransform.localPosition + (playerTransform.up * 2)) < boarRadius + playerRadius)) {
				GameHandler.gameHandler.actionComplete(Data.succesRate.negative);
				active = false;
			}

			if (Vector3.Distance(playerTransform.localPosition, Vector3.zero) > 1000f) {
				GameHandler.gameHandler.actionComplete(Data.succesRate.neutral);
				active = false;
			}
		}
	}
}