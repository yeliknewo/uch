using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int playerId = 1;
	public float walkStrength = 5.0f;
	public float jumpStrength = 10.0f;
	public float jumpCDBase = 0.2f;
	public bool canJump = true;
	private float jumpCD;

	void Start() {
		Object.FindObjectOfType<BuildPanel> ().AddPlayer(playerId);
		Object.FindObjectOfType<PlayerInputs> ().AddPlayer (playerId);
		Object.FindObjectOfType<CameraController> ().AddPlayer (gameObject);
	}

	void Update () {
		PlayerBuild build = Object.FindObjectOfType<BuildPanel> ().GetPlayerBuild (playerId);
		if (!build.IsBuilding () && !build.IsPlacing()) {
			PlayerInput input = Object.FindObjectOfType<PlayerInputs> ().GetPlayerInput (playerId);
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
