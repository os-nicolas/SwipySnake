using UnityEngine;
using System.Collections;

public class BranchSegment : Branch_Parent {

	LineRenderer line;
	EdgeCollider2D collider;


	public void Init(Vector3[] playerPath) {
		line = this.GetComponent<LineRenderer>();
		collider = this.GetComponent<EdgeCollider2D> ();
		transform.position = playerPath[0];
		Vector2[] collider_path = new Vector2[playerPath.Length];
		player_path = new Vector2[16];
		for (int i = 0; i < playerPath.Length; i++) {
			collider_path [i] = transform.InverseTransformPoint (playerPath[i]);
		}
		line.SetVertexCount (playerPath.Length);
		line.SetPositions (playerPath);
		collider.points = collider_path;
	}



}