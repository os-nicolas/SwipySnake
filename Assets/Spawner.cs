using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

<<<<<<< HEAD
	public GameObject player;
	public GameObject[] branches;

	// Use this for initialization
	void Start () {
		player = Instantiate(Resources.Load("Snake")) as GameObject;
		branches = new GameObject[12];
		//BranchController branch;
		for (int i = 0; i < 3; i++) {
			branches[i] = (GameObject) Instantiate (Resources.Load("branch_straight"));
			Vector3 bPos = branches[i].transform.position;
			bPos.x += 5 * (i-1);
			branches[i].transform.position = bPos;
		}
			player.GetComponent<SnakeController> ().currentBranch = branches[1];
=======
	public SnakeController player;
	public BranchController[] branches;
	public GameObject branch_prefab;

    // Use this for initialization
    void Start () {
		branches = new BranchController[3];
		//BranchController branch;
		for (int i = 0; i < 3; i++) {
            var branchGameObject = (GameObject)Instantiate(Resources.Load("Branch"));
            var branchController = branchGameObject.GetComponent<BranchController>();
            branchController.Init( new Vector2 (i * 5f, 0f));
			branches [i] = branchController;
		}
        var playerGameObject = (GameObject)Instantiate(Resources.Load("Snake"));;
        player = playerGameObject.GetComponent<SnakeController>();
        player.currentBranch = branches[1];
>>>>>>> b88915c488ea0085d16e8aab1d62e085b9e6a5eb
	}
	
	// Update is called once per frame
	void Update () {

	}

}
