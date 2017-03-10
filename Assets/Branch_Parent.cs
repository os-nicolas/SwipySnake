using UnityEngine;
using System.Collections;

public class Branch_Parent : MonoBehaviour {

	public Vector2[] player_path;
	public int age; // 0 => next, 1 => current, 2 => last
	public bool isCollidable;


	// Use this for initialization
	public Branch_Parent() {
		age = 0;
		isCollidable = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	bool AgeUp() {
		age++;
		return (age < 3); // return false if branch needs to be destroyed
	}

	//Return the next position for the snake to continue towards on the branch
	public float getNextPosition(float xPos) {
		int count = 0;
		int size = player_path.Length;
		while (player_path [count].x < xPos) {
			count++;
			if (count == size)
				return -999; // send snake onto next branch
		}
		return player_path [count].y;
	}


	public Vector2 getEndPosition() {
		return player_path [player_path.Length];
	}
}
