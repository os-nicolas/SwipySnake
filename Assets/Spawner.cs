using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public SnakeController player;
	public BranchController[] branches;
	// Use this for initialization
	void Start () {
		
		branches = new BranchController[3];
		for (int i = 0; i < 3; i++) {
			branches[i] = Instantiate(Resources.Load("Branch")) as BranchController;
			branches [i].nextPieceLocation = new Vector2 (i * 5, 0);
		}
		player = Instantiate(Resources.Load("Snake")) as SnakeController;
		//player.GetComponent<SnakeController> ().currentBranch = branches [1];
		player.currentBranch = branches[1];
	}
	
	// Update is called once per frame
	void Update () {

	}

}
