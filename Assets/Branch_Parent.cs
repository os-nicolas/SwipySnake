using UnityEngine;
using System.Collections;
using System;

public abstract class Branch_Parent : MonoBehaviour {

	public Vector2[] player_path;
	public int age; // 0 => next, 1 => current, 2 => last
	public bool isCollidable;
    public float branchSpeed = .1f;
	/*
	public bool toNextBranch =  false;
	public GameObject nextBranch;
	*/

	// Use this for initialization
	public Branch_Parent() {
		age = 0;
		isCollidable = true;
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	public bool AgeUp() {
		age++;
		return (age < 3);
	}
    
    /// <summary>
    /// Return the next position for the snake
    /// </summary>
    public Vector2 getNextPosition(Vector2 pos, float velocity) {
        var next = default(Vector2);
        //var last = default(Vector2);
		var closestDistance = float.MaxValue;

		for (int i = 0; i < player_path.Length - 1; i++) {
			var distL = (player_path [i] - pos).magnitude;
			if (distL < closestDistance) {
				next = player_path [i + 1];
				closestDistance = distL;
			}
		}

		//Move character towards next point
		var direction = (next - pos).normalized;
        return new Vector2(pos.x,pos.y) + (velocity * direction);
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
