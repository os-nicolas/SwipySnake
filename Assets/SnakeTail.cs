using UnityEngine;
using System.Collections;

public class SnakeTail : MonoBehaviour {

	public Vector3[] points;
	public float segmentLength;
	public LineRenderer line;

	// Use this for initialization
	void Start () {
		points = new Vector3 [10];
		segmentLength = .2f;
		line = this.GetComponent<LineRenderer> ();
		line.SetVertexCount (10);
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Redraw the tail to be the same length starting at the snake and going back.
	public void retraceTail (Vector3 newpoint) {
		newpoint.z = 0;
		Debug.Log ("New Point: " + newpoint.x + " " + newpoint.y);
		Vector3[] newPoints = new Vector3[10];
		newPoints [0] = newpoint;
		int j = 0;
		for (int i = 1; i < 10; i++) {
			bool done = false;
			while (j < 10 && !done) {
				float dist = Vector3.Distance (newPoints [i - 1], points [j]);
				// Maintain distance of 1
				if (dist == segmentLength) {
					newPoints [i] = points [j];
					j++;
					done = true;
					Debug.Log ("Adding Point");
				}
				// Create intermediate points
				else if (dist > segmentLength) {
					newPoints [i] = newPoints[i-1] + Vector3.Normalize(points [j] - newPoints [i - 1]) * segmentLength;
					Debug.Log ("Adding Middle Point " + newPoints[i].x + " " + newPoints[i].y);
					done = true;
				}
				// Move to next point
				else {
					Debug.Log ("Skipping point");
					j++;
				}
			}
			if (!done) {
				// Continue in same direction
				if (i >= 2) {
					newPoints [i] = newPoints[i-1] + Vector3.Normalize (newPoints [i - 1] - newPoints [i - 2]);
					Debug.Log ("Continuing in same direction");
				}
				// Continue Downwards
				else {
					newPoints [i] = new Vector3 (newPoints [i - 1].x, newPoints [i - 1].y - segmentLength);
					Debug.Log ("Went down: " + i);
				}
			}
		}
		line.SetPositions (newPoints);
		points = newPoints;
		for (int x = 0; x < 10; x++) {
			Debug.Log (points [x].x + " " + points [x].y);
			//line.SetPosition (x, points [x]);
		}
	}
}