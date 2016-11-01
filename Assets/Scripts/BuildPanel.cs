﻿using UnityEngine;
using System.Collections.Generic;

public class BuildPanel : MonoBehaviour {
	public GameObject canvas;

	private Dictionary<int, PlayerBuild> players;
	private List<UIObject> uiObjects;

	void Start () {
		players = new Dictionary<int, PlayerBuild> ();
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

	void Update() {
		PlayerInputs inputs = Object.FindObjectOfType<PlayerInputs> ();
		foreach (PlayerBuild player in players.Values) {
			int playerId = player.GetPlayerId ();
			PlayerInput input = inputs.GetPlayerInput (playerId);
			if (input.GetUp()) {
				MoveUp(playerId);
			}
			if (input.GetDown ()) {
				MoveDown (playerId);
			}
			if (input.GetLeft ()) {
				MoveLeft (playerId);
			}
			if (input.GetRight ()) {
				MoveRight (playerId);
			}
			if (GetPlayerBuild (playerId).IsBuilding ()) {
				if (input.GetSelect ()) {
					SelectBlock (playerId);
					input.SetPlaceCD (Time.time + 2.0f * input.GetPlaceCDBase ());
				}
				if (input.GetExit ()) { 
					InputExit (playerId);
					input.SetOpenBuildPanelCD (Time.time + input.GetOpenBuildPanelCDBase ());
				}
			} else if(!GetPlayerBuild(playerId).IsPlacing()) {
				if (input.GetOpenBuildPanel ()) {
					OpenBuildPanel (playerId);
					input.SetExitCD (Time.time + input.GetExitCDBase ());
				}
			}
		}
	}

	public void AddPlayer(int playerId) {
		GetPlayerBuild (playerId);
	}

	public PlayerBuild GetPlayerBuild(int playerId) {
		if (!players.ContainsKey (playerId)) {
			players.Add (playerId, new PlayerBuild (playerId));
		}

		return players[playerId];
	}

	private void ExitPanel() {
		foreach (PlayerBuild player in players.Values) {
			if (player.IsBuilding()) {
				return;
			}
		}
		canvas.SetActive (false);
	}
		
	private void OpenBuildPanel(int playerId) {
		canvas.SetActive (true);
		PlayerBuild player = GetPlayerBuild (playerId);
		player.SetBuilding (true);
		if (player.GetSelected () == null) {
			if (uiObjects.Count > 0) {
				player.SetSelected (uiObjects [0]);
			} else {
				Debug.Log ("UiObjects for Build Panel is Empty");
			}
		}
	}

	private void MoveLeft(int playerId) {
		PlayerBuild player = GetPlayerBuild (playerId);
		//Debug.Log ("Input Left");
		UIObject temp;
		//Debug.Log ("Selected: " + player.GetSelected ());
		temp = player.GetSelected ();
		if (temp != null) {
			//Debug.Log ("Left: " + temp.GetLeft());
			temp = temp.GetLeft ();
			if (temp != null) {
				player.SetSelected (temp);
			}
		}
	}

	private void MoveRight(int playerId) {
		PlayerBuild player = GetPlayerBuild (playerId);
		//Debug.Log ("Input Right");
		UIObject temp;
		//Debug.Log ("Selected: " + player.GetSelected ());
		temp = player.GetSelected ();
		if (temp != null) {
			//Debug.Log ("Right: " + temp.GetRight());
			temp = temp.GetRight ();
			if (temp != null) {
				player.SetSelected (temp);
			}
		}
	}

	private void MoveUp(int playerId) {
		PlayerBuild player = GetPlayerBuild (playerId);
		//Debug.Log ("Input Up");
		UIObject temp;
		//Debug.Log ("Selected: " + player.GetSelected ());
		temp = player.GetSelected ();
		if (temp != null) {
			//Debug.Log ("Up: " + temp.GetUp());
			temp = temp.GetUp ();
			if (temp != null) {
				player.SetSelected (temp);
			}
		}
	}

	private void MoveDown(int playerId) {
		PlayerBuild player = GetPlayerBuild (playerId);
		//Debug.Log ("Input Down");
		UIObject temp;
		//Debug.Log ("Selected: " + player.GetSelected ());
		temp = player.GetSelected ();
		if (temp != null) {
			//Debug.Log ("Down: " + temp.GetDown());
			temp = temp.GetDown ();
			if (temp != null) {
				player.SetSelected (temp);
			}
		}
	}

	private void SelectBlock(int playerId) {
		PlayerBuild player = GetPlayerBuild (playerId);
		if (player.GetSelected () != null) {
			player.SetPlacing (true);
			GameObject buildingBlock = Instantiate<GameObject> (player.GetSelected().GetBlock());
			buildingBlock.transform.position = Vector3.zero;
			buildingBlock.GetComponent<SpriteRenderer> ().color = player.GetSelectedColor (true);
			buildingBlock.AddComponent<BuildingBlock> ();
			buildingBlock.GetComponent<BuildingBlock> ().SetPlayer (playerId);
			InputExit (playerId);
		}
	}

	private void InputExit(int playerId) {
		PlayerBuild player = GetPlayerBuild (playerId);
		player.SetSelected (null);
		player.SetBuilding (false);
		ExitPanel ();
	}
}

public class PlayerBuild {
	private int playerId;
	private bool building;
	private bool placing;
	private UIObject selected;

	public PlayerBuild(int playerId) {
		this.playerId = playerId;
		this.building = false;
		this.placing = false;
	}

	internal void SetBuilding(bool building) {
		this.building = building;
	}

	public void SetPlacing(bool placing) {
		this.placing = placing;
	}

	internal void SetSelected(UIObject uiObject) {
		//Debug.Log ("Current Selected: " + this.selected + ", Next Selected: " + uiObject);
		if (this.selected != null) {
			this.selected.GetGameObject().GetComponent<CanvasRenderer> ().SetColor (this.GetSelectedColor (false));
		}
		this.selected = uiObject;
		if (uiObject != null) {
			this.selected.GetGameObject ().GetComponent<CanvasRenderer> ().SetColor (this.GetSelectedColor (true));
		}
	}

	public bool IsBuilding() {
		return this.building;
	}

	public bool IsPlacing() {
		return this.placing;
	}

	internal int GetPlayerId() {
		return this.playerId;
	}

	internal UIObject GetSelected() {
		return this.selected;
	}

	internal Color32 GetSelectedColor(bool selected) {
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
		return this.up;
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