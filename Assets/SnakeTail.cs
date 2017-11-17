using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeTail : MonoBehaviour {

	public Vector3[] points;
	public float segmentLength;
	public LineRenderer line;
    public float length;

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
    }
}