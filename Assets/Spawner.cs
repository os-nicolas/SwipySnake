﻿using UnityEngine;
using System.Collections;

// 6 7 8
// 3 4 5
// 0 1 2

//FIXME: No need for the tail to reinitialize, just move and reset it
public class Spawner : MonoBehaviour {

	public GameObject player;
	public GameObject[] branches;
	public GameObject[] obstacles;
	public GameObject fire;

	// Use this for initialization
	void Start () {
		
		branches = new GameObject[9];
		Vector3[][] branchGroup = new Vector3[3][];
		player = Instantiate(Resources.Load("Snake")) as GameObject;
		fire = Instantiate (Resources.Load ("FireLine")) as GameObject;
		//Initialize Starting Branches
		for (int i=0; i<3; i++) {
			//First Branch appears on starting screen
			branches [i] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [i].AddComponent<BranchStraight> ();
			Vector3 bPos = branches [i].transform.position;
			bPos.x = (i-1)*5;
			bPos.y = -5;
			branches [i].transform.position = bPos;
			branches [i].GetComponent<BranchStraight> ().Init (bPos);


			//Use getEndPosition() to get the location to put the next branch
			branches [i+3] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [i+3].AddComponent<BranchStraight> ();
			bPos = branches [i].GetComponent<Branch_Parent> ().getEndPosition ();
			branches [i+3].transform.position = bPos;
			branches [i+3].GetComponent<BranchStraight> ().Init (bPos);



			Vector3 endPos = branches [i+3].GetComponent<Branch_Parent> ().getEndPosition ();
			Vector3[] startEnd = new Vector3[2];
			startEnd [0] = endPos;
			endPos.y += 10;
			startEnd [1] = endPos;
			branchGroup [i] = startEnd;

			/*
			branches [i*3+2] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [i*3+2].AddComponent<BranchCurveLeft> ();
			bPos = branches [i*3+1].GetComponent<Branch_Parent> ().getEndPosition ();
			branches [i*3+2].transform.position = bPos;
			branches [i*3+2].GetComponent<BranchCurveLeft> ().Init (bPos);
			*/
		}
		branches [1].GetComponent<Branch_Parent> ().isCollidable = false;
		player.GetComponent<SnakeController> ().currentBranch = branches[1];


		branchGroup = generatePaths (branchGroup, 3);
		/*
		for (int i = 0; i < branchGroup [0].Length; i++) {
			Debug.Log("branchgroup " + branchGroup[0][i]);
		}
		*/
		for (int i = 0; i < 3; i++) {
			branches [i+6] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [i+6].AddComponent<BranchSegment> ();
			branches [i+6].GetComponent<BranchSegment> ().Init (branchGroup [i]);
		}



		player.GetComponent<SnakeController> ().currentBranch = branches [1];

		/*
		Vector3[][] paths = new Vector3[startPoints.Length] [2];
		for (int i=0; i<startPoints.Length; i++) {
			Vector3 start = startPoints [i];
			paths [i] [0] = start;
			paths [i] [1] = new Vector3 (start.x, Random(-5, 5), start.z);
		}
		Vector3[][] return_paths = generatePaths (paths, 0);
		*/
	}
	
	// Update is called once per frame
	void Update () {
		float camera_bottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
		if (player.GetComponent<SnakeController> ().die == true) {
			ResetGame ();
		}
		for (int i=0; i<3; i++) {
			Vector2 end = branches[i*3].GetComponent<Branch_Parent> ().getEndPosition ();
			if (end.y < camera_bottom) {
				ReplaceBranches (i);
			}
		}
		Vector3 firepos = fire.transform.position;
		if (firepos.y < camera_bottom + 2) {
			firepos.y = camera_bottom + 2;
		} else {
			firepos.y += .02f;
		}
		fire.transform.position = firepos;
		if (player.transform.position.y < firepos.y) {
			ResetGame ();
		}
	}

	void ReplaceBranches(int i) {
		GameObject b1 = branches [i];
		GameObject b2 = branches [i+1];
		GameObject b3 = branches [i+2];
		Destroy (b1);
		Destroy (b2);
		Destroy (b3);

		int x = -1;
		switch (i) {
		case 0:
			x = 6;
			break;
		case 1:
			x = 0;
			break;
		case 2:
			x = 3;
			break;
		}
		Vector3[][] branchGroup = new Vector3[3] [];
		Vector3 p1 = branches [x].GetComponent<Branch_Parent> ().getEndPosition ();
		Vector3 p2 = branches [x + 1].GetComponent<Branch_Parent> ().getEndPosition ();
		Vector3 p3 = branches [x + 2].GetComponent<Branch_Parent> ().getEndPosition ();

		branchGroup [0] [0] = p1;
		branchGroup [1] [0] = p2;
		branchGroup [2] [0] = p3;

		p1.y += 10;
		p2.y += 10;
		p3.y += 10;

		branchGroup [0] [1] = p1;
		branchGroup [1] [0] = p2;
		branchGroup [2] [1] = p3;


		branchGroup = generatePaths (branchGroup, 4);
		Vector3[][] branchGroup2 = new Vector3[3][];
		for (int j = 0; j < 3; j++) {
			branches [j+6] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [j+6].AddComponent<BranchSegment> ();
			branches [j+6].GetComponent<BranchSegment> ().Init (branchGroup [j]);
		}

		/*
		var rand = Random.Range (0, 2);
		GameObject old = branches [i];
		branches[i] = (GameObject)Instantiate (Resources.Load ("Branch"));
		if (rand < .5)
			branches [i].AddComponent<BranchStraight> ();
		else
			branches [i].AddComponent<BranchCurveLeft> ();
		Destroy (old);
		int last_index;
		switch (i) {
		case 0:
			last_index = 2;
			break;
		case 3:
			last_index = 5;
			break;
		case 6:
			last_index = 8;
			break;
		default:
			last_index = i - 1;
			break;
		}
		Vector3 bPos = branches [last_index].GetComponent<Branch_Parent> ().getEndPosition ();
		branches [i].transform.position = bPos;
		if (rand < .5) {
			branches [i].GetComponent<BranchStraight> ().Init (bPos);
		}
		else {
			branches [i].GetComponent<BranchCurveLeft> ().Init (bPos);
		}
		branches [i].GetComponent<Branch_Parent> ().addObstacles (.2f);
		*/
	}

	void ResetGame() {
		Destroy (player);
		Destroy (fire);
		for (int i = 0; i < 9; i++) {
			Destroy (branches [i]);
		}
		Start ();
	}

	//Generates a path for each branch with start and end points
	public Vector3[][] generatePaths(Vector3[][] branch_paths, int depth) {
		if (depth == 0)
			return branch_paths;

		var num_segs = branch_paths[0].Length;
		var num_bran = branch_paths.Length;

		Vector3[][] genPath = new Vector3[num_bran][];
		for (int i = 0; i < num_bran; i++) {
			genPath [i] = new Vector3[num_segs * 2 - 1]; 
		}

		//Iterate over each point of the branches
		for (int j = 0; j < num_segs-1; j++) {
			Vector3[] startPoints = new Vector3[num_bran];
			Vector3[] endPoints = new Vector3[num_bran];
			//Get the start and endpoints for each branch on this segment
			for (int i = 0; i < num_bran; i++) {
				startPoints [i] = branch_paths [i] [j];
				endPoints [i] = branch_paths [i] [j+1];
			}
			//Generate the midPoints for each branch together
			Vector3[] midPoints = genMidPoints (startPoints, endPoints);

			for (int i = 0; i < num_bran; i++) {
				genPath [i] [2 * j] = branch_paths [i] [j];
				genPath[i][2*j+1] = midPoints[i];
			}
		}
		//Add in last point
		for (int i = 0; i < num_bran; i++) {
			genPath [i] [num_segs * 2 - 2] = branch_paths [i] [num_segs - 1];
		}
		//Recursion
		return generatePaths (genPath, depth - 1);
	}


	public Vector3[] genMidPoints(Vector3[] startPoints, Vector3[] endPoints) {
		Vector3[] midPoints = new Vector3[startPoints.Length];

		for (int i = 0; i < startPoints.Length; i++) {
			Vector3 start = startPoints [i];
			Vector3 end = endPoints [i];
			var diff = end - start;
			var maxDist = Mathf.Sqrt(diff.magnitude);
			var center = (end + start) / 2f;
			var offset = Quaternion.Euler (0, 0, 90) * diff.normalized;

			Vector3 midpoint = center + (offset * Random.Range(-maxDist, maxDist));
			midPoints [i] = midpoint;
		}
		return midPoints;
	}
}
//0 1, 2 3
// 0 .5,  1 1.5, 2 2.5, 3 