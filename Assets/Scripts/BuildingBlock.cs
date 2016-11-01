using UnityEngine;
using System.Collections;

public class BuildingBlock : MonoBehaviour {
	private int playerId;
	private float speed = 10.0f;

	public void SetPlayer(int playerId) {
		this.playerId = playerId;
	}

	void Start() {
		GetComponent<BoxCollider2D> ().enabled = false;
	}

	void Update() {
		PlayerInput input = Object.FindObjectOfType<PlayerInputs> ().GetPlayerInput (playerId);
		Option<Vector2> vector = input.GetVector ();
		if (vector.IsSome ()) {
			transform.position = transform.position + (Vector3)vector.GetValue() * Time.deltaTime * speed;
		}
		if (input.GetPlace ()) {
			GetComponent<SpriteRenderer> ().color = Color.white;
			GetComponent<BoxCollider2D> ().enabled = true;
			Destroy (this);
			Object.FindObjectOfType<BuildPanel>().GetPlayerBuild(playerId).SetPlacing (false);
		}
	}
}
