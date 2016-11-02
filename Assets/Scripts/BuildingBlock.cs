using UnityEngine;
using System.Collections;

public class BuildingBlock : MonoBehaviour {
	private int playerId;
	private float speed = 10.0f;

	public bool initial;

	public void SetPlayer(int playerId) {
		this.playerId = playerId;
	}

	void Exit() {
		GetComponent<SpriteRenderer> ().color = Color.white;
		GetComponent<BoxCollider2D> ().enabled = true;
		Destroy (this);
	}

	void Start() {
		if (!initial) {
			GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	void Update() {
		if (initial) {
			return;
		}
		PlayerInput input = Object.FindObjectOfType<PlayerInputs> ().GetPlayerInput (playerId);
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
