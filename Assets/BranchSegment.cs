using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//PLAN: make into separate object from parent straight will now inherit from here 

public class BranchSegment : MonoBehaviour{

	LineRenderer	line;
	EdgeCollider2D	collider;

	public GameObject joint;
	public bool isEnd = false;

	public bool 		 isCollidable = true;
	public GameObject[]  obstacles;
	public List<Vector2> path;

	public float leftmost;
	public float rightmost;

	public float branchSpeed;

	void Awake() {
		line = this.GetComponent<LineRenderer> ();
		collider = this.GetComponent<EdgeCollider2D> ();
		branchSpeed = .1f;
	}

	public void setPath(List<Vector2> newPath, bool end) {
		path = new List<Vector2> ();
		int count = newPath.Count;
		Vector3[] vectors = new Vector3[count];
		Vector2[] collider_path = new Vector2[count];
		line.SetVertexCount (count);
		int x = 0;
		leftmost = float.MaxValue;
		rightmost = float.MinValue;
		foreach (Vector2 point in newPath) {
			path.Add (point);
			vectors [x] = new Vector3 (point.x, point.y, 0);
			collider_path [x] = new Vector2 (point.x, point.y);
			x++;
			if (point.x > rightmost)
				rightmost = point.x;
			if (point.x < leftmost)
				leftmost = point.x;
		}
		line.SetPositions (vectors);
		isEnd = end;
		collider.points = collider_path;
	}

	public void goStraight(Vector2 startPos) {
		List<Vector2> straight = new List<Vector2> ();
		Vector2 curPoint = startPos;
		for (int i = 0; i < 10; i++) {
			straight.Add (curPoint);
			curPoint.y += 1.75f;
		}
		setPath (straight, false);
	}

	/*
	public float Distance(Vector3 a, Vector3 b) {
		return Mathf.Sqrt(((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y)));
	}
	*/

	private Vector2 getNextPoint(Vector2 pos) {
		foreach (Vector2 point in path) {
			if (point.y > pos.y)
				return point;
		}
		return path.Last ();
	}

	public Vector2 getNextPosition(Vector2 pos, float velocity) {
		Vector2 next = getNextPoint (pos);
		
		var direction = (next - pos).normalized;
		var move = velocity * direction;
		Vector2 newPos = new Vector2 (pos.x, pos.y) + move;
		if (newPos.y > next.y) {
			if (isEnd)
				return pos;
		}
		return newPos;
		//FIXME: need to check for newPos.y > next.y in branch to get next seg
	}
		
	public Vector2 getEndPosition() {
		return path.Last();
	}

	public Vector2 generateSpread(float yDist) {
		Vector2 gen = path.Last();
		gen.y += yDist;
		gen.x += Random.Range (-3.0f, 3.0f);
		return gen;
	}
		/*
		var closest = default(Vector2);
		var next = default(Vector2);
		var last = default(Vector2);
		var closestDistance = float.MaxValue;

		var distL = default(float);

		foreach (Vector2 point in path) {
			distL = Distance(point, pos);
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
*/
		/*
		Vector2[] collider_path = new Vector2[playerPath.Length];
		Vector3[] line_path = new Vector3[playerPath.Length];
		player_path = new Vector2[playerPath.Length];

		for (int i = 0; i < playerPath.Length; i++) {
			Vector3 path_point = playerPath [i];
			path_point.z = 2;
			line_path[i] = path_point;
			player_path [i] = new Vector2 (path_point.x, path_point.y);
			collider_path [i] = transform.InverseTransformPoint (path_point);
		}
		line.SetVertexCount (line_path.Length);
		line.SetPositions (line_path);
		collider.points = collider_path;
		topJoint = Instantiate(Resources.Load("BranchJoint")) as GameObject;
		topJoint.transform.position = line_path [playerPath.Length-1];
	}
	*/


	/*
	void OnDestroy() {
		Destroy (topJoint);
	}
	*/
}