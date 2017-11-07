using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeTail : MonoBehaviour {

	public Vector3[] points;
	public float segmentLength;
	public LineRenderer line;
    public float length=5;

	// Use this for initialization
	void Awake () {
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
        List<Vector3> newPoints = new List<Vector3>();
        newPoints.Add(newpoint);

        var last = newpoint;
        var currentLength = 0.0;
        var at = 0;
        while (currentLength < length && at < points.Length) {
            var dis = Vector3.Distance(last, points[at]);
            currentLength += dis;
            newPoints.Add(points[at]);
            last = points[at];
            at++;
        }
        var newPointsArray = newPoints.ToArray();
        this.line.SetVertexCount(newPointsArray.Length);
        this.line.SetPositions(newPointsArray);
        points = newPointsArray;

        //      newpoint.z = 0;
        //Vector3[] newPoints = new Vector3[10];
        //newPoints [0] = newpoint;
        //int j = 0;
        //for (int i = 1; i < 10; i++) {
        //	bool done = false;
        //	while (j < 10 && !done) {
        //		float dist = Vector3.Distance (newPoints [i - 1], points [j]);
        //		// Maintain distance of 1
        //		if (dist == segmentLength) {
        //			newPoints [i] = points [j];
        //			j++;
        //			done = true;
        //		}
        //		// Create intermediate points
        //		else if (dist > segmentLength) {
        //			newPoints [i] = newPoints[i-1] + Vector3.Normalize(points [j] - newPoints [i - 1]) * segmentLength;
        //			done = true;
        //		}
        //		// Move to next point
        //		else {
        //			j++;
        //		}
        //	}
        //	if (!done) {
        //		// Continue in same direction
        //		if (i >= 2) {
        //			newPoints [i] = newPoints[i-1] + Vector3.Normalize (newPoints [i - 1] - newPoints [i - 2]);
        //		}
        //		// Continue Downwards
        //		else {
        //			newPoints [i] = new Vector3 (newPoints [i - 1].x, newPoints [i - 1].y - segmentLength);
        //		}
        //	}
        //}
        //this.line.SetVertexCount (newPoints.Length);
        //this.line.SetPositions (newPoints);
        //points = newPoints;
        /*
		for (int x = 0; x < 10; x++) {
			//Debug.Log (points [x].x + " " + points [x].y);
			//line.SetPosition (x, points [x]);
		}
		*/
    }
}