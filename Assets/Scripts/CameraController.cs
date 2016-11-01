using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {
	private List<GameObject> players;

	void Awake() {
		players = new List<GameObject> ();
	}
		
	void Update () {
		if(players.Count > 0){
			Vector3 temp = transform.position;
			float maxDistance = 0.0f;
			float buffer = 5.0f;
			Vector2 sum = Vector2.zero;
			for (int i = 0; i < players.Count; i++) {
				sum += (Vector2)players [i].transform.position;
				for (int j = i + 1; j < players.Count; j++) {
					maxDistance = Mathf.Max (maxDistance, Vector2.Distance (players [i].transform.position, players [j].transform.position));
				}
			}
			Vector2 ave = sum / players.Count;
			temp.x = ave.x;
			temp.y = ave.y;
			GetComponent<Camera> ().orthographicSize = maxDistance + buffer;
			transform.position = temp;
		}
	}

	public void AddPlayer(GameObject player) {
		players.Add (player);
	}
}
