using UnityEngine;
using System.Collections;
using System.Linq;

public abstract class Branch_Parent : MonoBehaviour {
    public const float branchZ = 0;

	public Vector2[] player_path;
	public int age; // 0 => next, 1 => current, 2 => last
	public bool isCollidable;
    public float branchSpeed = .1f;
	public GameObject[] obstacles;
    public Branch_Parent nextBranch;
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

    public float Distance(Vector3 a, Vector3 b) {
        return Mathf.Sqrt(((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y)));
    }

    /// <summary>
    /// Return the next position for the snake
    /// </summary>
    public Vector2 getNextPosition(Vector2 pos, float velocity) {
        var closest = default(Vector2);
        var next = default(Vector2);
        var last = default(Vector2);
		var closestDistance = float.MaxValue;


        var distL = default(float);

        for (int i = 1; i < player_path.Length - 1; i++) {
			distL = Distance(player_path [i],pos);
            if (distL < closestDistance)
            {
                last = player_path[i - 1];
                closest = player_path[i];
                next = player_path[i + 1];
                closestDistance = distL;
            }
        }

        
        if (nextBranch != null) {
            distL = Distance(nextBranch.player_path[0],pos);
            if (distL < closestDistance)
            {

                last = player_path[player_path.Length-2];
                closest = nextBranch.player_path[0];
                next = nextBranch.player_path[1];
                closestDistance = distL;
            }
        }

        var target = default(Vector2);
        
        distL = Distance(player_path[0], pos);
        if (distL < closestDistance)
        {
            closest = player_path[0];
            next = player_path[1];
            closestDistance = distL;
            target = next;
        }
        else {
            var dNext = Distance(pos, next) / Distance(closest,next);
            var dLast = Distance(pos, last) / Distance(closest, last);
            target = dNext < dLast + .1f ? next : closest;
        }
        

        //Move character towards next point
        var direction = (target - pos).normalized;
        var move = velocity * direction;

        return new Vector2(pos.x,pos.y) + move;
	}

    /// <summary>
    /// sets the player_path
    /// must be called before the branch can be used
    /// </summary>
    /// <param name="p">position of the branch</param>
    //public abstract void Init(Vector3 p);

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
