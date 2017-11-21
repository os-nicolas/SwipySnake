using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// 6 7 8
// 3 4 5
// 0 1 2

/*NOTE: 
 */

//FIXME: No need for the tail to reinitialize, just move and reset it
public class Spawner : MonoBehaviour {

	public GameObject player;
	public GameObject fire;
	public GameObject branchCtrl;

	// Use this for initialization
	void Start () {
		

		//Vector3[,] branchGroup = new Vector3[3, 2];
		player = Instantiate(Resources.Load("Snake")) as GameObject;
		fire = Instantiate (Resources.Load ("FireLine")) as GameObject;
		branchCtrl = Instantiate (Resources.Load ("BranchCtrl")) as GameObject;
		player.GetComponent<SnakeController> ().currentBranch = branchCtrl.GetComponent<BranchController> ().branches [1];
	}
	
	// Update is called once per frame
	void Update () {
		float camera_bottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
        float camera_top = Camera.main.transform.position.y + Camera.main.orthographicSize;
        if (player.GetComponent<SnakeController> ().die == true) {
			ResetGame ();
		}
		branchCtrl.GetComponent<BranchController>().trimBranches(camera_bottom, camera_top + 3);
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

	void ResetGame() {
		Destroy (player);
		Destroy (fire);
		Destroy (branchCtrl);
		Start ();
	}
	/*
	//void ReplaceBranches(int i) {
		//i indicates the row, i.e. 0->[0,1,2] etc.
		//Remove all of the branches from that row
		GameObject b1 = branches [i*3];
		GameObject b2 = branches [i*3+1];
		GameObject b3 = branches [i*3+2];
		Destroy (b1);
		Destroy (b2);
		Destroy (b3);

		//x determines which branch row we are placing the new branches above
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

		//branchGroup is the new set of branches, starting at the endpoint of row x
		Vector3[,] branchGroup = new Vector3[3, 2];

		//Get the starting location for the new branches 
		Vector3 p1 = branches [x].GetComponent<Branch_Parent> ().getEndPosition ();
		Vector3 p2 = branches [x + 1].GetComponent<Branch_Parent> ().getEndPosition ();
		Vector3 p3 = branches [x + 2].GetComponent<Branch_Parent> ().getEndPosition ();

		//Set the initial start and end values of each branch
		branchGroup [0, 0] = p1;
		branchGroup [1, 0] = p2;
		branchGroup [2, 0] = p3;
        var SectionHeight = 20; 
		p1.y += SectionHeight;
		p2.y += SectionHeight;
		p3.y += SectionHeight;
		int randX1 = Random.Range (-5, 5);
		int randX2;
		int randX3;
		do {
			randX2 = Random.Range(-5, 5);
		} while (Mathf.Abs(randX1-randX2) < 2);
		do {
			randX3 = Random.Range(-5, 5);
		} while (Mathf.Abs(randX1-randX3) < 2 || Mathf.Abs(randX2-randX3) < 2);
		p1.x = randX1;
		p2.x = randX2;
		p3.x = randX3;
		branchGroup [0, 1] = p1;
		branchGroup [1, 1] = p2;
		branchGroup [2, 1] = p3;

		//Call generatePaths to fill in the intermediary points
		branchGroup = generatePaths (branchGroup, 4);

		//Add the new branches to branches[]
		for (int j = 0; j < 3; j++) {
            var last = branches[x+j];
			branches [i*3+j] = (GameObject)Instantiate (Resources.Load ("Branch"));
			branches [i*3+j].AddComponent<BranchSegment> ();
			Vector3[] branchPoints = new Vector3[branchGroup.GetLength(1)];
			for (int y = 0; y < branchGroup.GetLength(1); y++) {
				branchPoints [y] = branchGroup [j, y];
			}
			branches [i*3+j].GetComponent<BranchSegment> ().Init (branchPoints);
            last.GetComponent<BranchSegment>().nextBranch = branches[i * 3 + j].GetComponent<BranchSegment>();
        }
	}
	*/


	/*
	//Generates a path for each branch with start and end points
	public Vector3[,] generatePaths(Vector3[,] branch_paths, int depth) {
		var num_segs = branch_paths.GetLength(1);
		var num_bran = branch_paths.GetLength(0);
		if (depth == 0) {
			int curvedLength = (num_segs * 3) - 1;
			Vector3[,] return_path = new Vector3[num_bran, curvedLength+1];
			//Curve each branch path
			for (int x = 0; x < num_bran; x++) {
				List<Vector3> points;

				List<Vector3> curvedPoints = new List<Vector3>(curvedLength);
				float t = 0.0f;
				for (int curvePoint = 0; curvePoint < curvedLength + 1; curvePoint++) {
					t = Mathf.InverseLerp (0, curvedLength, curvePoint);
					points = new List<Vector3> ();
					for (int y = 0; y < num_segs; y++) {
						points.Add (branch_paths [x, y]);
					}
					for (int j = num_segs - 1; j > 0; j--) {
						for (int i = 0; i < j; i++){
							points [i] = (1 - t) * points [i] + t * points [i + 1];
						}
					}
					curvedPoints.Add (points [0]);
				}

				Vector3[] path_array = curvedPoints.ToArray ();
				//Debug.Log (curvedLength);
				//Debug.Log (path_array.Length);
				for (int y = 0; y < path_array.Length; y++) {
					return_path [x, y] = path_array [y];
				}
			}
			return return_path;
		}

		Vector3[,] genPath = new Vector3[num_bran, num_segs * 2 - 1];

		//Iterate over each point of the branches
		for (int j = 0; j < num_segs-1; j++) {
			Vector3[] startPoints = new Vector3[num_bran];
			Vector3[] endPoints = new Vector3[num_bran];
			//Get the start and endpoints for each branch on this segment
			for (int i = 0; i < num_bran; i++) {
				startPoints [i] = branch_paths [i, j];
				endPoints [i] = branch_paths [i, j+1];
			}
			//Generate the midPoints for each branch together
			Vector3[] midPoints = genMidPoints (startPoints, endPoints);

			for (int i = 0; i < num_bran; i++) {
				genPath [i, 2 * j] = branch_paths [i, j];
				genPath [i, 2*j+1] = midPoints[i];
			}
		}
		//Add in last point
		for (int i = 0; i < num_bran; i++) {
			genPath [i, num_segs * 2 - 2] = branch_paths [i, num_segs - 1];
		}
		//Recursion
		return generatePaths (genPath, depth - 1);
	}


	public Vector3[] genMidPoints(Vector3[] startPoints, Vector3[] endPoints) {
		Vector3[] midPoints = new Vector3[startPoints.Length * 3 - 1];

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
	*/
}