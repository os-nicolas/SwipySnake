using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject player;
	public GameObject[] branches;
	// Use this for initialization
	void Start () {
		player = Instantiate(Resources.Load("Snake")) as GameObject;
		branches = new GameObject[12];
		for (int i = 0; i < 3; i++) {
			branches[i] = (GameObject)Instantiate(Resources.Load("branch_straight"));
			Vector3 bPos = branches [i].transform.position;
			bPos.x += 5 * i;
			branches [i].transform.position = bPos;
		}
		player.GetComponent<SnakeController> ().currentBranch = branches [1];
	}
	
	// Update is called once per frame
	void Update () {

	}

}
