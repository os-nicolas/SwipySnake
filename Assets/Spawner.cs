using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject player;
	public GameObject[] branches;

	// Use this for initialization
	void Start () {
		player = Instantiate(Resources.Load("Snake")) as GameObject;
		branches = new GameObject[9];
		//Initialize Starting Branches
		for (int i=0; i<3; i++) {
			//First Branch appears on starting screen
			branches [i*3] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [i*3].AddComponent<BranchStraight> (); 
			Vector3 bPos = branches [i*3].transform.position;
			bPos.x = (i-1)*5;
			bPos.y = -5;
			branches [i*3].transform.position = bPos;
			branches [i*3].GetComponent<BranchStraight> ().Init (bPos);

			//Use getEndPosition() to get the location to put the next branch
			branches [i*3+1] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [i*3+1].AddComponent<BranchCurveLeft> ();
			bPos = branches [i*3].GetComponent<Branch_Parent> ().getEndPosition ();
			branches [i*3+1].transform.position = bPos;
			branches [i*3+1].GetComponent<BranchCurveLeft> ().Init (bPos);

			branches [i*3+2] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [i*3+2].AddComponent<BranchCurveLeft> ();
			bPos = branches [i*3+1].GetComponent<Branch_Parent> ().getEndPosition ();
			branches [i*3+2].transform.position = bPos;
			branches [i*3+2].GetComponent<BranchCurveLeft> ().Init (bPos);
		}
		branches [3].GetComponent<Branch_Parent> ().isCollidable = false;
		player.GetComponent<SnakeController> ().currentBranch = branches[3];
	}
	
	// Update is called once per frame
	void Update () {
		float camera_bottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
		for (int i=0; i<9; i++) {
			Vector2 end = branches[i].GetComponent<Branch_Parent> ().getEndPosition ();
			if (end.y < camera_bottom) {
				ReplaceBranch (i);
			}
		}
	}

	void ReplaceBranch(int i) {
		var rand = Random.Range (0, 2);
		Debug.Log (rand);
		GameObject old = branches [i];
		branches[i] = (GameObject)Instantiate (Resources.Load ("Branch"));
		if (rand < .5)
			branches [i].AddComponent<BranchStraight> ();
		else
			branches [i].AddComponent<BranchCurveLeft> ();
		Destroy (old);
		int last_index;
		switch (i) {
		case 0:
			last_index = 2;
			break;
		case 3:
			last_index = 5;
			break;
		case 6:
			last_index = 8;
			break;
		default:
			last_index = i - 1;
			break;
		}
		Vector3 bPos = branches [last_index].GetComponent<Branch_Parent> ().getEndPosition ();
		branches [i].transform.position = bPos;
		if (rand < .5) {
			branches [i].GetComponent<BranchStraight> ().Init (bPos);
		}
		else {
			branches [i].GetComponent<BranchCurveLeft> ().Init (bPos);
		}
	}
}