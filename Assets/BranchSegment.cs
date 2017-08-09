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
	}
}