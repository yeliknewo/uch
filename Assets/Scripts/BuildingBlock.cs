using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BuildingBlock : MonoBehaviour {
	private int playerId;
	private float speed = 10.0f;

	public void SetPlayer(int playerId, Color color) {
		this.playerId = playerId;
		GetComponent<SpriteRenderer> ().color = color;
		GetComponent<BoxCollider2D> ().enabled = false;
		Object.FindObjectOfType<BuildPanel> ().GetPlayerBuild (playerId).SetPlacing (true);
	}

	void Exit() {
		GetComponent<SpriteRenderer> ().color = Color.white;
		GetComponent<BoxCollider2D> ().enabled = true;
		Destroy (this);
	}

	void Update() {
		PlayerInputs inputs = Object.FindObjectOfType<PlayerInputs> ();
		if (inputs == null) {
			Exit ();
			return;
		}
		PlayerInput input = inputs.GetPlayerInput (playerId);
		if (input == null) {
			Exit ();
			return;
		}
		Option<Vector2> vector = input.GetVector ();
		if (vector.IsSome ()) {
			transform.position = transform.position + (Vector3)vector.GetValue() * Time.deltaTime * speed;
		}
		if (input.GetPlace ()) {
			Exit ();
			Object.FindObjectOfType<BuildPanel>().GetPlayerBuild(playerId).SetPlacing (false);
		}
	}
}
