using UnityEngine;
using System.Collections;

public abstract class Branch_Parent : MonoBehaviour {

	public Vector2[] player_path;
	public int age; // 0 => next, 1 => current, 2 => last
	public bool isCollidable;
    public float branchSpeed = .1f;
	public GameObject[] obstacles; 
	/*
	public bool toNextBranch =  false;
	public GameObject nextBranch;
	*/

	// Use this for initialization
	public Branch_Parent() {
		age = 0;
		isCollidable = true;
		obstacles = new GameObject[5];
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
        var closest = default(Vector2);
        //var last = default(Vector2);
		var closestDistance = float.MaxValue;

		for (int i = 0; i < player_path.Length - 1; i++) {
			var distL = (player_path [i] - pos).magnitude;
			if (distL < closestDistance) {
                closest = player_path[i];
				next = player_path [i + 1];
				closestDistance = distL;
			}
		}

		//Move character towards next point
		var direction = ((next - pos).normalized + (next- closest).normalized*1.1f).normalized;
        var move = velocity * direction;
        return new Vector2(pos.x,pos.y) + move;
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

	/*
	void OnDestroy() {
		foreach (GameObject g in obstacles)
			Destroy (g);
	}
	*/

	public void addObstacles(float probability) {
		
		int count = 0;
		int spacing = 0;
		for (int i=0; i<player_path.Length; i++) {
			spacing++;
			if (spacing == 5 && Random.value < probability && count<5) {
				count++;
				spacing = 0;
				obstacles[count] = (GameObject)Instantiate (Resources.Load ("Obstacle"));
				obstacles [count].transform.position = player_path[i];
			}
		}

	}
    
}
