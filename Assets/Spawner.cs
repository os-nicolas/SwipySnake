using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

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
            branchController.nextPieceLocation = new Vector2 (i * 5f, 0f);
			branches [i] = branchController;
		}
        var playerGameObject = (GameObject)Instantiate(Resources.Load("Snake"));;
        player = playerGameObject.GetComponent<SnakeController>();
        player.currentBranch = branches[1];
	}
	
	// Update is called once per frame
	void Update () {

	}

}
