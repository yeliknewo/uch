using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs: MonoBehaviour
{
	private Dictionary<int, PlayerInput> inputs = new Dictionary<int, PlayerInput> ();

	public void AddPlayer(int playerId) {
		if (!inputs.ContainsKey (playerId)) {
			inputs.Add (playerId, new PlayerInput (playerId));
		}
	}

	public PlayerInput GetPlayerInput(int playerId) {
		if (inputs.ContainsKey(playerId)) {
			return inputs [playerId];
		} else {
			return null;
		}
	}
}

public class PlayerInput {

	private int playerId;

	private const string JUMP = InputConsts.A;
	private const string SELECT = InputConsts.X;
	private const string EXIT = InputConsts.Y;
	private const string X_AXIS = InputConsts.DPAD_X_AXIS;
	private const string Y_AXIS = InputConsts.DPAD_Y_AXIS;
	private const string PLACE = InputConsts.X;
	private const string OPEN_BUILD_PANEL = InputConsts.Y;

	private float x_deadzone;
	private float y_deadzone;

	private float jumpCD;
	private float selectCD;
	private float exitCD;
	private float xAxisCD;
	private float yAxisCD;
	private float vectorCD;
	private float placeCD;
	private float upCD;
	private float downCD;
	private float leftCD;
	private float rightCD;
	private float openBuildPanelCD;

	public PlayerInput(int playerId, float x_deadzone = 0.3f, float y_deadzone = 0.3f) {
		this.playerId = playerId;
		this.x_deadzone = x_deadzone;
		this.y_deadzone = y_deadzone;
	}

	public void SetJumpCD(float cd) {
		jumpCD = cd;
	}

	public void SetSelectCD(float cd) {
		selectCD = cd;
	}

	public void SetExitCD(float cd) {
		exitCD = cd;
	}

	public void SetXAxisCD(float cd) {
		xAxisCD = cd;
	}

	public void SetYAxisCD(float cd) {
		yAxisCD = cd;
	}

	public void SetVectorCD(float cd) {
		vectorCD = cd;
	}

	public void SetPlaceCD(float cd) {
		placeCD = cd;
	}

	public void SetUpCD(float cd) {
		upCD = cd;
	}

	public void SetDownCD(float cd) {
		downCD = cd;
	}

	public void SetLeftCD(float cd) {
		leftCD = cd;
	}

	public void SetRightCD(float cd) {
		rightCD = cd;
	}

	public void SetOpenBuildPanelCD(float cd) {
		openBuildPanelCD = cd;
	}

	public float GetJumpCD() {
		return jumpCD;
	}

	public float GetSelectCD() {
		return selectCD;
	}

	public float GetXAxisCD() {
		return xAxisCD;
	}

	public float GetYAxisCD() {
		return yAxisCD;
	}

	public float GetVectorCD() {
		return vectorCD;
	}

	public float GetUpCD() {
		return upCD;
	}

	public float GetDownCD() {
		return downCD;
	}

	public float GetLeftCD() {
		return leftCD;
	}

	public float GetRightCD() {
		return rightCD;
	}

	public float GetPlaceCD() {
		return placeCD;
	}

	public float GetExitCD() {
		return exitCD;
	}

	public float GetOpenBuildPanelCD() {
		return openBuildPanelCD;
	}

	public float GetJumpCDBase() {
		return 0.01f;
	}

	public float GetXAxisCDBase() {
		return 0.01f;
	}

	public float GetYAxisCDBase() {
		return 0.01f;
	}

	public float GetVectorCDBase() {
		return 0.01f;
	}

	public float GetUpCDBase() {
		return 0.1f;
	}

	public float GetDownCDBase() {
		return 0.1f;
	}

	public float GetLeftCDBase() {
		return 0.1f;
	}

	public float GetRightCDBase() {
		return 0.1f;
	}

	public float GetSelectCDBase() {
		return 0.1f;
	}

	public float GetExitCDBase() {
		return 0.1f;
	}

	public float GetPlaceCDBase() {
		return 0.1f;
	}

	public float GetOpenBuildPanelCDBase() {
		return 0.1f;
	}

	public bool GetJump() {
		if (Time.time >= GetJumpCD () && GetJumpForced ()) {
			SetJumpCD (Time.time + GetJumpCDBase ());
			return true;
		}
		return false;
	}

	public bool GetUp() {
		if (Time.time >= GetUpCD () && GetUpForced ()) {
			SetUpCD (Time.time + GetUpCDBase ());
			return true;
		}
		return false;
	}

	public bool GetDown() {
		if (Time.time >= GetDownCD () && GetDownForced ()) {
			SetDownCD (Time.time + GetDownCDBase ());
			return true;
		}
		return false;
	}

	public bool GetLeft() {
		if (Time.time >= GetLeftCD () && GetLeftForced ()) {
			SetLeftCD (Time.time + GetLeftCDBase ());
			return true;
		}
		return false;
	}

	public bool GetRight() {
		if (Time.time >= GetRightCD () && GetRightForced ()) {
			SetRightCD (Time.time + GetRightCDBase ());
			return true;
		}
		return false;
	}

	public bool GetSelect() {
		if (Time.time >= GetSelectCD () && GetSelectForced ()) {
			SetSelectCD (Time.time + GetSelectCDBase ());
			return true;
		}
		return false;
	}

	public bool GetExit() {
		if (Time.time >= GetExitCD () && GetExitForced ()) {
			SetExitCD (Time.time + GetExitCDBase ());
			return true;
		}
		return false;
	}

	public bool GetPlace() {
		if (Time.time >= GetPlaceCD () && GetPlaceForced()) {
			SetPlaceCD (Time.time + GetPlaceCDBase ());
			return true;
		}
		return false;
	}

	public bool GetOpenBuildPanel() {
		if (Time.time >= GetOpenBuildPanelCD () && GetOpenBuildPanelForced ()) {
			SetOpenBuildPanelCD (Time.time + GetOpenBuildPanelCDBase ());
			return true;
		}
		return false;
	}

	public Option<float> GetXAxis() {
		if (Time.time >= GetXAxisCD()) {
			SetXAxisCD (Time.time + GetXAxisCDBase ());
			return new Option<float>(GetXAxisForced());
		}
		return new Option<float>();
	}

	public Option<float> GetYAxis() {
		if (Time.time >= GetYAxisCD ()) {
			SetYAxisCD (Time.time + GetYAxisCDBase ());
			return new Option<float>(GetYAxisForced ());
		}
		return new Option<float>();
	}

	public Option<Vector2> GetVector() {
		if (Time.time >= GetVectorCD ()) {
			SetVectorCD (Time.time + GetVectorCDBase ());
			return new Option<Vector2> (GetVectorForced ());
		}
		return new Option<Vector2> ();
	}

	public bool GetJumpForced() {
		return Input.GetButton (JUMP + playerId) || GetYAxisForced () > y_deadzone;
	}

	public float GetXAxisForced() {
		return Input.GetAxis (X_AXIS + playerId);
	}

	public float GetYAxisForced() {
		return Input.GetAxis (Y_AXIS + playerId);
}

	public Vector2 GetVectorForced() {
		return new Vector2 (GetXAxisForced (), GetYAxisForced ());
	}

	public bool GetUpForced() {
		return GetYAxisForced () > y_deadzone;
	}

	public bool GetDownForced() {
		return GetYAxisForced () < -y_deadzone;
	}

	public bool GetLeftForced() {
		return GetXAxisForced () < -x_deadzone;
	}

	public bool GetRightForced() {
		return GetXAxisForced () > x_deadzone;
	}

	public bool GetSelectForced() {
		return Input.GetButton (SELECT + playerId);
	}

	public bool GetExitForced() {
		return Input.GetButton (EXIT + playerId);
	}

	public bool GetPlaceForced() {
		return Input.GetButton (PLACE + playerId);
	}

	public bool GetOpenBuildPanelForced() {
		return Input.GetButton (OPEN_BUILD_PANEL + playerId);
	}
}

public class Option<T> {
	private bool full;
	private T value;

	public Option(T value) {
		this.value = value;
		this.full = true;
	}

	public Option() {
		this.full = false;
	}

	public bool IsSome() {
		return this.full;
	}

	public T GetValue() {
		return this.value;
	}
}
