using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 	BranchController handles the generation and deletion of branch objects
/// 
///		Mainly handles branches is a list of continuous branches (cb), with each
/// 	cb containing all of its connected pieces with no gaps inbetween. When a
/// 	gap is reached, a new cb must be created.
/// </summary>

public class BranchController : MonoBehaviour {

	public List<GameObject> branches;
	//public GameObject[] obstacles;
	private int curveHeight = 5;
	private int sectionCurves = 4;

	void Awake () {
		branches = new List<GameObject> ();
		for (int i = 0; i < 3; i++) {
			GameObject br = GameObject.Instantiate (Resources.Load ("Branch")) as GameObject;
			//br.AddComponent<Branch> ();
			Vector2 p = new Vector2 ((i - 1) * 5, -5.0f);
			br.GetComponent<Branch> ().addStraight (p);
			if (i == 1) {
				br.GetComponent<Branch> ().segments.Last().GetComponent<BranchSegment>().isCollidable = false;
			}
			branches.Add (br);
		}
		//generatePaths ();
	}


	/*  1: Get the endpoints of all active paths (1, 3, 5) 
	 *  2: Generate next point for each path ((1,0), (3,4), (5,3))
	 *  3: Determine overlaps and generate new branches
	 * 		 paths: ((1,0), (4), (3))  Add: ((3), (5))
	 * 
	 * ALTERNATIVE
	 * 1: Generate the entire new path for each branch ((1,0,3,1), (3, 4, 7, 3), (5, 3, 1, 0))
	 * 2: Determine overlaps and divide
	 * 		Overlaps: (1-3: 3, 2-3: 2) -> (1:(1,0)2:(3)3:(5)(3)) + (1:(3,1)2:(4,7,3))3:(1,0))
	 * 										capped 					not capped
	 * 
	 * Need to also account for overlaps when wrapping the screen
	 * Say one point is at 0 and another at 7 with screen size of 6
	 *   The wrapped point will be at 1
	 *   So for point (4->7) 6 overlaps exist:
	 * 		(5->3) Right to left (a1>b1 && a2<b2)
	 * 		(3->8) Left to Right (a1<b1 && a2>b2)
	 *   Wrapped overlaps (-2, 1) and (10, 13)
	 * 		(0->-1) Right to Left (a1<b1 && a2>b2)
	 * 
	 * 
	 * FIXME: This will probably generate more branches then are being deleted!
	 * 
	 */

	public void generatePaths() {
		List<List<Vector2>> openPaths = new List<List<Vector2>> ();
		List<List<Vector2>> cappedPaths = new List<List<Vector2>> ();
		//Start at endpoints for all ongoing branches
		foreach (GameObject cb in branches) {
			GameObject br = cb.GetComponent<Branch>().segments.Last ();
			if (!br.GetComponent<BranchSegment> ().isEnd) {
				List<Vector2> n = new List<Vector2> ();
				n.Add (br.GetComponent<BranchSegment> ().getEndPosition());
				openPaths.Add (n);
				br.GetComponent<BranchSegment> ().isEnd = true;
			}
		}

		//Generate new paths
		foreach (List<Vector2> p in openPaths) {
			for (int i = 0; i < sectionCurves; i++) {
				Vector2 next = p.Last ();
				next.y += 1f;
				next.x += Random.Range (-3.0f, 3.0f);
				p.Add (next);
			}
		}

		//Split newOpenPaths into newCappedPaths
		for (int i = 0; i < openPaths.Count - 1; i++) {
			HashSet<int> overlaps = new HashSet<int> ();
			for (int j = i + 1; j < openPaths.Count; j++) {
				overlaps.UnionWith(findOverlaps (openPaths [i], openPaths [j]));
			}
			foreach (int o in overlaps) {
				cappedPaths.AddRange (openPaths.GetRange (0, o));
				openPaths.RemoveRange (0, o);
			}
		}

		//Create Branches
		foreach (List<Vector2> p in openPaths) {
			GameObject br = GameObject.Instantiate (Resources.Load ("Branch")) as GameObject; 
			br.GetComponent<Branch> ().addSegment (p, true);
			//GameObject br = new GameObject (p.First ());
			//br.addSegment (p, false);
			//branches.Add (br);
		}
	}

	private List<int> findOverlaps(List<Vector2> a, List<Vector2> b) {
		List<int> overlaps = new List<int>();
		for (int i = 1; i < a.Count; i++) {
			for (int j = 0; j < 3; j++) {
				float x1 = a [i - 1].x + (6*j - 6);
				float x2 = a [i].x + (6*j - 6);
				float y1 = b [i - 1].x;
				float y2 = b [i].x;
				if (!overlaps.Contains(i)
					&& (x1 == y1 || x2 == y2
				    || (x1 < y1 && x2 > y2)
					|| (x1 > y1 && x2 < y2))) {
					overlaps.Add (i);
				}
			}
		}
		return overlaps;
	}

	public void trimPaths(float y){
		for (int i = branches.Count-1; i>=0; i--) {
			GameObject br = branches [i];
			if (br.GetComponent<Branch>().trimBottom (y)) {
				branches.RemoveAt (i);
				Destroy(br);
			}
		}
	}

}

		/*
		//newPaths holds the completed paths to make new branches from
		List<List<float>> newPaths = new List<List<float>> ();
		//genPaths holds the paths currently being generated
		List<List<float>> genPaths = new List<List<float>> ();
		foreach (Branch cb in branches) {
			//Get the last branchSegment to start from
			GameObject br = cb.Last ();
			//If the branch continues, add another segment
			if (!br.GetComponent<BranchSegment>().isEnd) {
				List<float> l = new List<float>();
				l.Add(br.GetComponent<BranchSegment>().getEndPosition().x);
				genPaths.Add(l);
			}
		}
			
		for (int i = 0; i < sectionCurves; i++) {
			List<float> curPoints = new List<float>();
			List<float> newPoints = new List<float>();
			//Populate newPoints with the next x-level y-coordinates for the path
			foreach (List<float> p in genPaths) {
				float nextPoint = genPaths.Last () + Random.Range (-3.0f, 3.0f);
				newPoints.Add (nextPoint);
				curPoints.Add (genPaths.Last ());
				//float nextPoint = newPaths [j].Last () + Random.Range (-3.0f, 3.0f);
				//genPoints.Add(new float[] { nextPoint, newPaths [j].Last () });
			}

			for (int j = 0; j < newPoints.Count; j++) {
				for (int k = j+1; k < newPoints.Count; k++) {
					if ( curPoints[i] == curPoints[j] || newPoints[i] == newPoints[j]
						|| (curPoints[j] < curPoints[k] && newPoints[j] > newPoints[k])
						|| (curPoints[j] > curPoints[k] && newPoints[j] < newPoints[k])) {

					}
				}
			}


			List<int> makeNewIndexes = new List<int> ();
			for (int j = genPoints.Count; j>=0; j--) {
				for (int k = j-1; k >= 0; k--) {
					if (genPoints[j][0] > genPoints[k][0] && 
					}
				}
			}
			
		}





		//List of all the branches currently being added to
		List<GameObject> activeBr = new List<GameObject> ();
		//Find all active branches and add new segment
		foreach(Branch cb in branches) {
			GameObject br = cb.Last();
			if (!br.GetComponent<BranchSegment>().isEnd) {
				activeBr.Add (br);
			}
		}

		//Generate paths for new Segments
		for (int i = 0; i < sectionCurves; i++) {
			List<int[]> newPoints = new List<int[]> ();
			foreach (GameObject nb in activeBr) {
				newPoints.Add(new int[] {nb.GetComponent<BranchSegment> ().generateSpread ().x, 0});
			}
			foreach (int[] newP in newPoints) {
				//Step 1 determine if overlapping\
				//Step 2 cap all overlapping and make new branch
			}
		}
		*/
		//Initialize all new branches at the very end