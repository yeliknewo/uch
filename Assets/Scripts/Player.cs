using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {
	public int playerId = 1;
	public float walkStrength = 5.0f;
	public float jumpStrength = 10.0f;
	public float jumpCDBase = 0.2f;
	public bool canJump = true;

	public GameObject buildPanelPrefab;
	public GameObject playerInputsPrefab;
	private float jumpCD;
	private BuildPanel buildPanel;
	private PlayerInputs playerInputs;

	private bool setup = false;

	public override void OnStartAuthority() {
		CmdSetup ();
	}

	[Command]
	void CmdSetup() {
		GameObject playerInputsObj = Instantiate<GameObject> (playerInputsPrefab);
		playerInputs = GetComponent<PlayerInputs> ();
		playerInputs.AddPlayer (playerId);
		NetworkServer.Spawn (playerInputsObj);
		GameObject buildPanelObj = Instantiate<GameObject> (buildPanelPrefab);
		buildPanel = buildPanelObj.GetComponent<BuildPanel> ();
		buildPanel.AddPlayer (playerId, gameObject);
		NetworkServer.SpawnWithClientAuthority (buildPanelObj, gameObject);
		Object.FindObjectOfType<CameraController> ().AddPlayer (gameObject);
		setup = true;
	}

	void Update () {
		if (!setup) {
			if (hasAuthority) {
				CmdSetup ();
			}
		}
		if(!isLocalPlayer || !hasAuthority) {
			return;
		}
		PlayerBuild build = buildPanel.GetPlayerBuild (playerId);
		if (!build.IsBuilding () && !build.IsPlacing()) {
			PlayerInput input = playerInputs.GetPlayerInput (playerId);
			Option<float> xAxis = input.GetXAxis ();
			if (xAxis.IsSome ()) {
				this.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * xAxis.GetValue() * walkStrength);
			}
			if (input.GetJump ()) {
				this.TryJump ();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (canJump) {
			return;
		}
		if (coll.gameObject.tag == Tags.BLOCK) {
			foreach (ContactPoint2D point in coll.contacts) {
				//Debug.Log ("Point: " + point.point);
				//Debug.Log ("Pos: " + transform.position.y);
				//Debug.Log ("Ext Y: " + coll.collider.bounds.extents.y * transform.lossyScale.y);
				//Debug.Log ("Point - (Pos + Ext Y): " + (point.point.y - (transform.position.y + coll.collider.bounds.extents.y * transform.lossyScale.y)));
				if ((point.point.y - (transform.position.y + coll.collider.bounds.extents.y * transform.lossyScale.y)) < 0.0f) {
					SetCanJump (true);
					break;
				}
			}
		}
	}

	void SetCanJump(bool jump) {
		if (Time.time > jumpCD) {
			canJump = jump;
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (!canJump) {
			OnCollisionEnter2D (coll);
		}
	}

	void TryJump() {
		if (canJump && Time.time > jumpCD) {
			Jump ();
		}
	}

	void Jump() {
		jumpCD = Time.time + jumpCDBase;
		canJump = false;
		GetComponent<Rigidbody2D> ().AddForce (Vector2.up * jumpStrength, ForceMode2D.Impulse);
	}
}
