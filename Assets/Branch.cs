using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Branch : MonoBehaviour {

    
	public List<GameObject> segments;
	//public List<GameObject> joints;
	//public bool isCapped = false;


	void Awake () {
		segments = new List<GameObject> ();
	}

	public void addStraight (Vector2 startPos) {
		GameObject br = GameObject.Instantiate (Resources.Load ("BranchSegment")) as GameObject;
		br.AddComponent<BranchSegment>();
		br.GetComponent<BranchSegment> ().goStraight (startPos);
		segments.Add (br);
	}

	public void addSegment(List<Vector2> path, bool isEnd) {
		GameObject br = (GameObject)GameObject.Instantiate (Resources.Load ("BranchSegment"));
		br.AddComponent<BranchSegment>();
		br.GetComponent<BranchSegment> ().setPath(path, isEnd);
		segments.Add (br);
	}


	void OnDestroy() {
		foreach (GameObject g in segments)
			Destroy (g);
	}

	//Returns true if it should be destroyed
	public bool trimBottom(float yPos) {
		for (int i = segments.Count-1; i>=0; i--) {
			GameObject seg = segments [i];
			if (seg.GetComponent<BranchSegment>().getEndPosition().y < yPos) {
				segments.RemoveAt (i);
				GameObject.Destroy(seg);
			}
		}
		if (segments.Count == 0)
			return true;
		return false;
	}

	/*
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
	*/
    
}
