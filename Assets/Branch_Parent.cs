using UnityEngine;
using System.Collections;

public abstract class Branch_Parent : MonoBehaviour {

	public Vector2[] player_path;
	public int curPoint;
	public bool finished = false;
    public BranchController parent_branch;
	public int myIndex;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Return the next position for the snake to continue towards on the branch
	public Vector2 getNextPoint() {
		Debug.Assert(!finished);
		return player_path [curPoint];
	}

	public void incrementPoint() {
		curPoint++;
		if (curPoint == player_path.Length) {
			finished = true;
		}
	}

    /// <summary>
    /// sets the player_path
    /// must be called before the branch can be used
    /// </summary>
    /// <param name="p">position of the branch</param>
    public abstract void Init(Vector3 p);

	public Vector2 getEndPosition() {
		return player_path [player_path.Length];
	}
}
