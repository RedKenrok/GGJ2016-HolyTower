using UnityEngine;

public struct PlayerInput {

    private sbyte Vertical;
    public sbyte vertical {
        get {
            return Vertical;
        }
    }

	private bool VerticalKeyDown;
	public bool verticalKeyDown {
		get {
			return VerticalKeyDown;
		}
	}

    private sbyte Horizontal;
    public sbyte horizontal {
        get {
            return Horizontal;
        }
	}

	private bool HorizontalKeyDown;
	public bool horizontalKeyDown {
		get {
			return HorizontalKeyDown;
		}
	}
    
    private bool Accept;
    public bool accept {
        get {
            return Accept;
        }
	}

	private bool AcceptKeyDown;
	public bool acceptKeyDown {
		get {
			return AcceptKeyDown;
		}
	}

    private bool Decline;
    public bool decline {
        get {
            return Decline;
        }
	}

	private bool DeclineKeyDown;
	public bool declineKeyDown {
		get {
			return DeclineKeyDown;
		}
	}

	public PlayerInput(sbyte ver, bool verKeyDown, sbyte hor, bool horKeyDown, bool acc, bool accKeyDown, bool dec, bool decKeyDown) {
        Vertical = ver;
		VerticalKeyDown = verKeyDown;
        Horizontal = hor;
		HorizontalKeyDown = horKeyDown;
        Accept = acc;
		AcceptKeyDown = accKeyDown;
        Decline = dec;
		DeclineKeyDown = decKeyDown;
    }
}

public class InputHandler : MonoBehaviour {

	private static float countdownVertical = 0;
	private static float countdownHorizontal = 0;
	private static float delay = 0.4f;

    public static PlayerInput GetInput() {
		sbyte vertical = (sbyte) Mathf.Clamp(Input.GetAxis("KeyboardVertical") - Input.GetAxis("JoyAxis5"), -1, 1);
		bool verticalKeyDown = Input.GetButtonDown("KeyboardVertical");

		bool verticalPadDown = (Input.GetAxis("JoyAxis5") != 0) ? true : false;
		if (verticalPadDown && countdownVertical <= 0) {
			verticalKeyDown = verticalKeyDown || verticalPadDown;
			countdownVertical = delay;
		}

		sbyte horizontal = (sbyte) Mathf.Clamp(Input.GetAxis("KeyboardHorizontal") + Input.GetAxis("JoyAxis1"), -1, 1);
		bool horizontalKeyDown = Input.GetButtonDown("KeyboardHorizontal");

		bool horizontalPadDown = (Input.GetAxis("JoyAxis1") != 0) ? true : false;
		if (horizontalPadDown && countdownHorizontal <= 0) {
			horizontalKeyDown = horizontalKeyDown || horizontalPadDown;
			countdownHorizontal = delay;
		}

		bool accept = Input.GetButton("KeyboardA") || Input.GetButton("SnesA");
		bool acceptKeyDown = Input.GetButtonDown("KeyboardA") || Input.GetButtonDown("SnesA");

		bool decline = Input.GetButton("KeyboardB") || Input.GetButton("SnesB");
		bool declineKeyDown = Input.GetButtonDown("KeyboardB") || Input.GetButtonDown("SnesB");

		return new PlayerInput(vertical, verticalKeyDown, horizontal, horizontalKeyDown, accept, acceptKeyDown, decline, declineKeyDown);
    }

	private void Update() {
		countdownVertical -= (countdownVertical > 0) ? Time.deltaTime : 0;
		countdownHorizontal -= (countdownHorizontal > 0) ? Time.deltaTime : 0;
	}
}