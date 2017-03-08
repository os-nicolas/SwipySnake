using UnityEngine;
using System.Collections;

public class Branch_Parent : MonoBehaviour {

	public Vector2[] player_path;
	public int curPoint;
	public bool finished;
	public BranchController parent_branch;
	public int myIndex;


	// Use this for initialization
	void Start () {
		finished = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Return the next position for the snake to continue towards on the branch
	public Vector2 getNextPoint() {
		Debug.Assert (!finished);
		return player_path [curPoint];
	}

	public void incrementPoint() {
		curPoint++;
		if (curPoint == player_path.Length) {
			finished = true;
		}
	}


	public Vector2 getEndPosition() {
		return player_path [player_path.Length];
	}
}
