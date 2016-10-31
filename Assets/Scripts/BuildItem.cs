using UnityEngine;
using System.Collections;

public class BuildItem : MonoBehaviour {
	public GameObject left;
	public GameObject right;
	public GameObject up;
	public GameObject down;
	public GameObject block;
	private int id;

	private static int nextId = 0;

	void Start() {
		id = nextId++;
	}

	public int GetId() {
		return id;
	}
}
