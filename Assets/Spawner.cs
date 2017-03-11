using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject player;
	public GameObject[] branches;

	// Use this for initialization
	void Start () {
		player = Instantiate(Resources.Load("Snake")) as GameObject;
		GameObject[] branches = new GameObject[9];

		for (int i=0; i<3; i++) {

			branches [i*3] = (GameObject)Instantiate (Resources.Load ("branch_straight"));
			Vector3 bPos = branches [i*3].transform.position;
			bPos.x = (i-1)*5;
			branches [i*3].transform.position = bPos;
			branches [i*3].GetComponent<BranchStraight> ().Init (bPos);

			branches [i*3+1] = (GameObject)Instantiate (Resources.Load ("branch_straight"));
			bPos = branches [i*3].GetComponent<Branch_Parent> ().getEndPosition ();
			branches [i*3+1].transform.position = bPos;
			branches [i*3+1].GetComponent<BranchStraight> ().Init (bPos);
		}
		player.GetComponent<SnakeController> ().currentBranch = branches[1];
	}
	
	// Update is called once per frame
	void Update () {

	}

}
