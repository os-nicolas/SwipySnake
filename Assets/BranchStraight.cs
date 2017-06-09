using UnityEngine;
using System.Collections;

public class BranchStraight : Branch_Parent {

	//Vector3 p;

	// Use this for initialization
	LineRenderer line;
	EdgeCollider2D collider;

	public BranchStraight () {
	}
    

	public void Init(Vector3 p) {
		line = this.GetComponent<LineRenderer>();
		collider = this.GetComponent<EdgeCollider2D> ();
        var count = 10;
		line.SetVertexCount(count);
		transform.position = p;
		Vector2 pos = new Vector2(p.x, p.y);
		player_path = new Vector2[count];
		Vector2[] collider_path = new Vector2[count];
		Vector3[] line_path = new Vector3[count];
		for (int i = 0; i < count; i++) {
			player_path [i] = pos;
			line_path [i] = new Vector3 (pos.x, pos.y, 2);
			collider_path [i] = transform.InverseTransformPoint (line_path[i]);
			pos.y += 1.75f;
		}
		line.SetPositions (line_path);
		//collider.pointCount = line_path.Length; //this did not work
		collider.points = collider_path;
	}
}