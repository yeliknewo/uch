using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject[] players;

	void Update () {

		if(players.Length > 0){
			Vector3 temp = transform.position;
			float maxDistance = 0.0f;
			float buffer = 5.0f;
			Vector2 sum = Vector2.zero;
			for (int i = 0; i < players.Length; i++) {
				sum += (Vector2)players [i].transform.position;
				for (int j = i + 1; j < players.Length; j++) {
					maxDistance = Mathf.Max (maxDistance, Vector2.Distance (players [i].transform.position, players [j].transform.position));
				}
			}
			Vector2 ave = sum / players.Length;
			temp.x = ave.x;
			temp.y = ave.y;
			GetComponent<Camera> ().orthographicSize = maxDistance + buffer;
			transform.position = temp;
		}

	}
}
