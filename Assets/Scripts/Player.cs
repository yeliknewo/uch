using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int joystickId = 1;
	public float walkStrength = 5.0f;
	public float jumpStrength = 10.0f;

	public bool canJump = true;

	private const string JUMP = InputConsts.A;
	private const string SELECT = InputConsts.A;
	private const string EXIT = InputConsts.X;
	private const string X_AXIS = InputConsts.DPAD_X_AXIS;
	private const string Y_AXIS = InputConsts.DPAD_Y_AXIS;

	private const float Y_DEADZONE = 0.3f;
	private const float X_DEADZONE = 0.3f;

	private BuildPanel panel;

	void Start() {
		panel = Object.FindObjectOfType<BuildPanel> ();
	}

	private bool GetInputJump() {
		return Input.GetButton (JUMP + joystickId);
	}

	private float GetInputXAxis() {
		return Input.GetAxis (X_AXIS + joystickId);
	}

	private float GetInputYAxis() {
		return Input.GetAxis (Y_AXIS + joystickId);
	}

	private Vector2 GetInputVector() {
		return new Vector2 (GetInputXAxis (), GetInputYAxis ());
	}

	private bool GetInputUp() {
		return GetInputYAxis () > Y_DEADZONE;
	}

	private bool GetInputDown() {
		return GetInputYAxis () < -Y_DEADZONE;
	}

	private bool GetInputLeft() {
		return GetInputXAxis () < -X_DEADZONE;
	}

	private bool GetInputRight() {
		return GetInputXAxis () > X_DEADZONE;
	}

	private bool GetInputSelect() {
		return Input.GetButton (SELECT + this.joystickId);
	}

	private bool GetInputExit() {
		return Input.GetButton (EXIT + this.joystickId);
	}

	void Update () {
		GetComponent<Rigidbody2D> ().AddForce (Vector2.right * GetInputXAxis () * walkStrength);
		if (GetInputYAxis () > Y_DEADZONE || GetInputJump ()) {
			this.TryJump ();
		}
		if (GetInputUp ()) {
			panel.InputUp (joystickId);
		}
		if (GetInputDown ()) {
			panel.InputDown (joystickId);
		}
		if (GetInputLeft ()) {
			panel.InputLeft (joystickId);
		}
		if (GetInputRight ()) {
			panel.InputRight (joystickId);
		}
		if (GetInputSelect ()) {
			panel.InputSelect (joystickId);
		}
		if (GetInputExit ()) { 
			panel.InputExit (joystickId);
		}
	}

	void TryJump() {
		if (canJump) {
			Jump ();
		}
	}

	void Jump() {
		canJump = false;
		GetComponent<Rigidbody2D> ().AddForce (Vector2.up * jumpStrength, ForceMode2D.Impulse);
	}
}
