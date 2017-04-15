﻿using UnityEngine;
using System.Collections;

public class BranchStraight : Branch_Parent {

	//Vector3 p;

	// Use this for initialization
	LineRenderer line;
	EdgeCollider2D collider;

	public BranchStraight () {
	}
    

	public override void Init(Vector3 p) {
		line = this.GetComponent<LineRenderer>();
		collider = this.GetComponent<EdgeCollider2D> ();
		line.SetVertexCount (10);
		//p.y += 4;
		transform.position = p;
		Vector2 pos = new Vector2(p.x, p.y);
		//pos.y -= 8;
		player_path = new Vector2[10];
		Vector2[] collider_path = new Vector2[10];
		Vector3[] line_path = new Vector3[10];
		for (int i = 0; i < 10; i++) {
			player_path [i] = pos;
			line_path [i] = new Vector3 (pos.x, pos.y, 1);
			collider_path [i] = transform.InverseTransformPoint (line_path[i]);
			pos.y += 1.75f;
		}
		line.SetPositions (line_path);
		//collider.pointCount = line_path.Length; //this did not work
		collider.points = collider_path;
	}
}