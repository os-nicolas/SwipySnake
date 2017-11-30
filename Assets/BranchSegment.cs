using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class BranchSegment : MonoBehaviour{

	LineRenderer	line;
	EdgeCollider2D	collider;

	public GameObject joint;
	public bool active = false;

	public bool 		 isCollidable = true;
	//public GameObject[]  obstacles;
	public List<Vector2> path;

	public float leftmost;
	public float rightmost;
    public GameObject wrapCopy;

	public float branchSpeed;

	void Awake() {
		line = this.GetComponent<LineRenderer> ();
		collider = this.GetComponent<EdgeCollider2D> ();
		branchSpeed = .1f;
	}

    public List<Vector2> getWrapPath(float xShift)
    {
        List<Vector2> newPath = new List<Vector2>();
        foreach (Vector2 point in path)
            newPath.Add(new Vector2(point.x + xShift, point.y));
        return newPath;
    }

	public void setPath(List<Vector2> newPath, bool setActive) {
		path = new List<Vector2> ();
		int count = newPath.Count;
		Vector3[] vectors = new Vector3[count];
		Vector2[] collider_path = new Vector2[count];
        line.positionCount = count;
        leftmost = float.MaxValue;
		rightmost = float.MinValue;
		for (int i = 0; i< newPath.Count; i++) {
            Vector2 point = newPath[i];
			path.Add (point);
			vectors [i] = new Vector3 (point.x, point.y, 0);
			collider_path [i] = new Vector2 (point.x, point.y);
			if (point.x > rightmost)
				rightmost = point.x;
			if (point.x < leftmost)
				leftmost = point.x;
		}
		line.SetPositions (vectors);
		active = setActive;
		collider.points = collider_path;
	}

	public void goStraight(Vector2 startPos) {
		List<Vector2> straight = new List<Vector2> ();
		Vector2 curPoint = startPos;
		for (int i = 0; i < 10; i++) {
			straight.Add (curPoint);
			curPoint.y += 1.75f;
		}
		setPath (straight, true);
	}
    

	private Vector3 getNextPoint(Vector3 pos) {
		foreach (Vector2 point in path) {
            if (point.y > pos.y)
            {
                return new Vector3(point.x, point.y, pos.z);
            }
		}
        var last = path.Last();
        return new Vector3(last.x, last.y, pos.z);

	}

	public Vector3 getNextPosition(Vector3 pos, float velocity) {
        Vector3 next = getNextPoint (pos);
        if (pos.y >= next.y)
        {
            return pos;
        }
        var direction = (next - pos).normalized;
		var move = direction * velocity;
		Vector3 newPos = pos + move;
		
		return newPos;
	}
		
	public Vector2 getEndPosition() {
		return path.Last();
	}


	/*
	void OnDestroy() {
		Destroy (topJoint);
	}
	*/
}