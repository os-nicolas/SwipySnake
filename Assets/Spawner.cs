using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public SnakeController player;
	public GameObject[] branches;
	public GameObject branch_prefab;
	// Use this for initialization
	void Start () {
		branches = new BranchController[3];
		//BranchController branch;
		for (int i = 0; i < 3; i++) {
			GameObject branch = Instantiate (branch_prefab, Vector3.zero, Quaternion.identity);
			branch.AddComponent<BranchController>();
			branch.GetComponent<BranchController>().nextPieceLocation = new Vector2 (i * 5f, 0f);
			branches [i] = branch;
		}
		player = Instantiate(Resources.Load("Snake")) as SnakeController;
		//player.GetComponent<SnakeController> ().currentBranch = branches [1];
		player.currentBranch = branches[1];
	}
	
	// Update is called once per frame
	void Update () {

	}

}
