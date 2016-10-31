using UnityEngine;
using System.Collections.Generic;

public class BuildPanel : MonoBehaviour {
	public GameObject canvas;

	private Dictionary<int, Players> players;
	private List<UIObject> uiObjects;

	void Start () {
		players = new Dictionary<int, Players> ();
		uiObjects = new List<UIObject> ();
		canvas.SetActive (false);
		foreach (BuildItem item in canvas.GetComponentsInChildren<BuildItem> ()) {
			uiObjects.Add (new UIObject (item.GetId(), item.gameObject, item.block));
		}
		foreach (BuildItem item in canvas.GetComponentsInChildren<BuildItem> ()) {
			UIObject uiObject = null;
			foreach (UIObject ui in uiObjects) {
				if (ui.GetId () == item.GetId ()) {
					uiObject = ui;
					break;
				}
			}
			if (uiObject == null) {
				Debug.Log ("Unable to find the object I just made");
				return;
			}
			foreach (UIObject other in uiObjects) {
				if (item.left != null && other.GetId() == item.left.GetComponent<BuildItem>().GetId()) {
					uiObject.SetLeft (other);
				} 
				if (item.right != null && other.GetId() == item.right.GetComponent<BuildItem>().GetId()) {
					uiObject.SetRight (other);
				}
				if (item.up != null && other.GetId() == item.up.GetComponent<BuildItem>().GetId()) {
					uiObject.SetUp (other);
				}
				if (item.down != null && other.GetId() == item.down.GetComponent<BuildItem>().GetId()) {
					uiObject.SetDown (other);
				}
			}
		}
	}

	private Players GetPlayers(int playerId) {
		if (!players.ContainsKey (playerId)) {
			players.Add (playerId, new Players (playerId));
		}

		return players[playerId];
	}

	private void ExitPanel() {
		foreach (Players player in players.Values) {
			if (player.IsPlayerBuilding()) {
				return;
			}
		}
		canvas.SetActive (false);
	}
		
	private void OpenBuildPanel(int playerId) {
		canvas.SetActive (true);
		Players player = GetPlayers (playerId);
		player.SetPlayerBuilding (true);
		if (player.GetSelected () == null) {
			if (uiObjects.Count > 0) {
				player.SetSelected (uiObjects [0]);
			} else {
				Debug.Log ("UiObjects for Build Panel is Empty");
			}
		}
	}

	public void InputLeft(int playerId) {
		Players player = GetPlayers (playerId);
		if (!player.CanDoInput ()) {
			return;
		}
		player.DoInput ();
		Debug.Log ("Input Left");
		UIObject temp;
		Debug.Log ("Selected: " + player.GetSelected ());
		temp = player.GetSelected ();
		if (temp != null) {
			Debug.Log ("Left: " + temp.GetLeft());
			temp = temp.GetLeft ();
			if (temp != null) {
				player.SetSelected (temp);
			}
		}
	}

	public void InputRight(int playerId) {
		Players player = GetPlayers (playerId);
		if (!player.CanDoInput ()) {
			return;
		}
		player.DoInput ();
		Debug.Log ("Input Right");
		UIObject temp;
		Debug.Log ("Selected: " + player.GetSelected ());
		temp = player.GetSelected ();
		if (temp != null) {
			Debug.Log ("Right: " + temp.GetRight());
			temp = temp.GetRight ();
			if (temp != null) {
				player.SetSelected (temp);
			}
		}
	}

	public void InputUp(int playerId) {
		Players player = GetPlayers (playerId);
		if (!player.CanDoInput ()) {
			return;
		}
		player.DoInput ();
		Debug.Log ("Input Up");
		UIObject temp;
		Debug.Log ("Selected: " + player.GetSelected ());
		temp = player.GetSelected ();
		if (temp != null) {
			Debug.Log ("Up: " + temp.GetUp());
			temp = temp.GetUp ();
			if (temp != null) {
				player.SetSelected (temp);
			}
		}
	}

	public void InputDown(int playerId) {
		Players player = GetPlayers (playerId);
		if (!player.CanDoInput ()) {
			return;
		}
		player.DoInput ();
		Debug.Log ("Input Down");
		UIObject temp;
		Debug.Log ("Selected: " + player.GetSelected ());
		temp = player.GetSelected ();
		if (temp != null) {
			Debug.Log ("Down: " + temp.GetDown());
			temp = temp.GetDown ();
			if (temp != null) {
				player.SetSelected (temp);
			}
		}
	}

	public void InputSelect(int playerId) {
		Players player = GetPlayers (playerId);
		if (!player.CanDoInput ()) {
			return;
		}
		player.DoInput ();
		if (!player.IsPlayerBuilding ()) {
			OpenBuildPanel (playerId);
		} else {
			if (player.GetSelected () != null) {
				GameObject buildingBlock = Instantiate<GameObject> (player.GetSelected().GetBlock());
				buildingBlock.transform.position = Vector3.zero;
				buildingBlock.GetComponent<SpriteRenderer> ().color = player.GetSelectedColor (true);
				buildingBlock.AddComponent<BuildingBlock> ();
				player.SetPlayerBuilding (false);
				player.SetSelected (null);
				ExitPanel ();
			}
		}
	}

	public void InputExit(int playerId) {
		Players player = GetPlayers (playerId);
		if (!player.CanDoInput ()) {
			return;
		}
		player.DoInput ();
		player.SetSelected (null);
		player.SetPlayerBuilding (false);
		ExitPanel ();
	}
}

class Players {
	private const float INPUT_DELAY = 0.1f;

	private int playerId;
	private bool playerBuilding;
	private UIObject selected;
	private float timeToAllowInput;

	public Players(int playerId) {
		this.playerId = playerId;
		this.playerBuilding = false;
		this.timeToAllowInput = 0.0f;
	}

	public void DoInput() {
		timeToAllowInput = Time.time + INPUT_DELAY;
	}

	public bool CanDoInput() {
		return Time.time > timeToAllowInput;
	}

	public void SetPlayerBuilding(bool building) {
		this.playerBuilding = building;
	}

	public void SetSelected(UIObject uiObject) {
		Debug.Log ("Current Selected: " + this.selected + ", Next Selected: " + uiObject);
		if (this.selected != null) {
			this.selected.GetGameObject().GetComponent<CanvasRenderer> ().SetColor (this.GetSelectedColor (false));
		}
		this.selected = uiObject;
		if (uiObject != null) {
			this.selected.GetGameObject ().GetComponent<CanvasRenderer> ().SetColor (this.GetSelectedColor (true));
		}
	}

	public bool IsPlayerBuilding() {
		return this.playerBuilding;
	}

	public int GetPlayerId() {
		return this.playerId;
	}

	public UIObject GetSelected() {
		return this.selected;
	}

	public Color32 GetSelectedColor(bool selected) {
		if (!selected) {
			return Color.white;
		}
		switch (playerId) {
		case 1:
			return Color.red;

		case 2:
			return Color.blue;

		case 3:
			return Color.green;

		case 4:
			return Color.yellow;

		default:
			return Color.white;
		}
	}
}

class UIObject {
	private int id;
	private GameObject gameObject;
	private GameObject block;
	private UIObject left;
	private UIObject right;
	private UIObject up;
	private UIObject down;

	public UIObject(int id, GameObject gameObject, GameObject block, UIObject left = null, UIObject right = null, UIObject up = null, UIObject down = null) {
		this.id = id;
		this.gameObject = gameObject;
		this.block = block;
		this.left = left;
		this.right = right;
		this.up = up;
		this.down = down;
	}

	public void SetLeft(UIObject left) {
		this.left = left;
	}

	public void SetRight(UIObject right) {
		this.right = right;
	}

	public void SetUp(UIObject up) {
		this.up = up;
	}

	public void SetDown(UIObject down) {
		this.down = down;
	}

	public UIObject GetLeft() {
		return this.left;
	}

	public UIObject GetRight() {
		return this.right;
	}

	public UIObject GetUp() {
		return this.down;
	}

	public UIObject GetDown() {
		return this.down;
	}

	public GameObject GetBlock() {
		return this.block;
	}

	public GameObject GetGameObject() {
		return this.gameObject;
	}

	public int GetId() {
		return this.id;
	}
}