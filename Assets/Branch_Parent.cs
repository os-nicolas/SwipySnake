using UnityEngine;
using System.Collections;

public abstract class Branch_Parent : MonoBehaviour {

	public Vector2[] player_path;
<<<<<<< HEAD
	public int age; // 0 => next, 1 => current, 2 => last
	public bool isCollidable;


	// Use this for initialization
	public Branch_Parent() {
		age = 0;
		isCollidable = true;
=======
	public int curPoint;
	public bool finished = false;
    public BranchController parent_branch;
	public int myIndex;


	// Use this for initialization
	void Start () {
>>>>>>> b88915c488ea0085d16e8aab1d62e085b9e6a5eb
	}
	
	// Update is called once per frame
	void Update () {

	}

<<<<<<< HEAD
	bool AgeUp() {
		age++;
		return (age < 3); // return false if branch needs to be destroyed
=======
	//Return the next position for the snake to continue towards on the branch
	public Vector2 getNextPoint() {
		Debug.Assert(!finished);
		return player_path [curPoint];
>>>>>>> b88915c488ea0085d16e8aab1d62e085b9e6a5eb
	}

	//Return the next position for the snake to continue towards on the branch
	public float getNextPosition(float yPos) {
		int count = 0;
		int size = player_path.Length;
		while (player_path [count].y < yPos) {
			count++;
			if (count == size)
				return -999; // send snake onto next branch
		}
		return player_path [count].x;
	}

    /// <summary>
    /// sets the player_path
    /// must be called before the branch can be used
    /// </summary>
    /// <param name="p">position of the branch</param>
    public abstract void Init(Vector3 p);

	public Vector2 getEndPosition() {
		return player_path [player_path.Length-1];
	}
}
