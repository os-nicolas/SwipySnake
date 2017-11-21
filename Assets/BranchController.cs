using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 	BranchController handles the generation and deletion of branch objects
/// 	TODO:   Make continuous generation
/// 	        Test and push changes
/// 	        Merge
/// 	        Readd obstacles
/// 	        Readd joints
/// 	        wrapping!
/// </summary>

public class BranchController : MonoBehaviour {

    public List<GameObject> branches;
    //public GameObject[] obstacles;
    private int curveHeight = 5;
    private int sectionCurves = 4;
    private float SectionHeight = 20.0f;

    void Awake() {
        branches = new List<GameObject>();
        for (int i = 0; i < 3; i++) {
            GameObject br = Instantiate(Resources.Load("BranchSegment")) as GameObject;
            Vector2 p = new Vector2((i - 1) * 5, -5.0f);
            br.GetComponent<BranchSegment>().goStraight(p);
            if (i == 1) {
                br.GetComponent<BranchSegment>().isCollidable = false;
            }
            branches.Add(br);
        }
        generateBranches();
    }

    //Remove any branches which have gone below the screen
    public void trimBottom(float yPos)
    {
        for (int i = branches.Count - 1; i >= 0; i--)
        {
            GameObject seg = branches[i];
            if (seg.GetComponent<BranchSegment>().getEndPosition().y < yPos)
            {
                branches.RemoveAt(i);
                GameObject.Destroy(seg);
            }
        }
    }

    //Find intersection points for two branches
    //TODO: switch to List<Vector2>
    public List<List<int>> getIntersects(List<Vector2> a, List<Vector2> b)
    {
        List<int> Aintersects = new List<int>();
        List<int> Bintersects = new List<int>();
        List<List<int>> intersects = new List<List<int>>();
        int x = 0;
        int y = 0;
        char aSide = 'N'; //Start in Neutral position
        Vector2 aPos = a.First();
        Vector2 bPos = b.First();
        //Check only the points where the two sets begin to overlap
        if (aPos.y < bPos.y)
        {
            while (x < a.Count - 1 && a[x + 1].y < bPos.y)
            {
                x++;
            }
            if (a[x + 1].y < bPos.y)
                return intersects;
        }
        else if (aPos.y > bPos.y)
        {
            while (y < b.Count - 1 && b[y + 1].y < aPos.y)
            {
                y++;
            }
            if (b[y + 1].y < aPos.y)
                return intersects;
        }

        //Find overlap points 
        while (x < a.Count && y < b.Count)
        {
            aPos = a[x];
            bPos = b[y];
            //Step 1: Check the new position of the branches to find overlaps
            //Case 1: the two branches meet, always consider as an overlap
            if (aPos.x == bPos.x)
            {
                aSide = 'N';
                if (!Aintersects.Contains(x))
                {
                    Aintersects.Add(x);
                }
                if (!Bintersects.Contains(y))
                {
                    Bintersects.Add(y);
                }
            }
            //Case 2: a is on the left side
            else if (aPos.x < bPos.x)
            {
                //If a was on right before add intersect
                if (aSide == 'R')
                {
                    if (!Aintersects.Contains(x)) {
                        Aintersects.Add(x);
                    }
                    if (!Bintersects.Contains(y)) {
                        Bintersects.Add(y);
                    }
                }
                aSide = 'L';
            }
            //Case 3: a is on the right
            else
            {
                //If a was on left before add intersect
                if (aSide == 'L')
                {
                    if (!Aintersects.Contains(x)) {
                        Aintersects.Add(x);
                    }
                    if (!Bintersects.Contains(y)) {
                        Bintersects.Add(y);
                    }
                }
                aSide = 'R';
            }

            //Step 2: Check whether the next points x-coordinates align
            //Case 1: Same height check and increase both
            if (aPos.y == bPos.y)
            {
                x++;
                y++;
            }
            //Case 2: aPos is below bPos check and increase aPos
            else if (aPos.y < bPos.y)
            {
                x++;
            }
            //Case 3: aPos is above bPos check and increase bPos
            else
            {
                y++;
            }

        }
        intersects.Add(Aintersects);
        intersects.Add(Bintersects);
        return intersects;
    }

    //TODO: This might get removed!
    public void addBranches(List<List<Vector2>> newBranches, bool active)
    {
        foreach (List<Vector2> branchPath in newBranches)
        {
            GameObject br = GameObject.Instantiate(Resources.Load("BranchSegment")) as GameObject;
            br.GetComponent<BranchSegment>().setPath(branchPath, active);
            branches.Add(br);
        }
    }

    //Takes a list of branches and separates them there they overlap
    //TODO: switch to List<List<Vector2>> and finish
    public List<List<List<Vector2>>> splitGaps(List<List<Vector2>> branch_paths)
    {

        List<List<int>> gaps = new List<List<int>>();
        for (int i = 0; i < branch_paths.Count; i++)
        {
            gaps.Add(new List<int>());
        }

        for (int i = 0; i < branch_paths.Count; i++)
        {
            for (int j = i + 1; j < branch_paths.Count; j++)
            {
                var g = getIntersects(branch_paths[i], branch_paths[j]);
                gaps[i] = gaps[i].Union<int>(g[0]).ToList<int>();
                gaps[j] = gaps[j].Union<int>(g[1]).ToList<int>();
            }
        }

        List<List<List<Vector2>>> newBranches = new List<List<List<Vector2>>>();
        newBranches.Add(new List<List<Vector2>>()); //active branches
        newBranches.Add(new List<List<Vector2>>()); //inactive branches

        for (int i = 0; i < gaps.Count; i++)
        {
            int start = 0;
            for (int j = 0; j < gaps[i].Count - 1; j++)
            {
                if (gaps[i][j] - start > 0)
                    newBranches[1].Add(branch_paths[i].GetRange(start, gaps[i][j] - start));
                start = gaps[i][j] + 1;
            }
            newBranches[0].Add(branch_paths[i].GetRange(start, branch_paths[i].Count - start));
        }

        return newBranches;
    }

    //Creates new branches at the endpoint of each currently open branch
    public void generateBranches()
    {
        List<Vector2> endPoints = new List<Vector2>();

        foreach (GameObject cb in branches)
        {
            if (cb.GetComponent<BranchSegment>().active)
            {
                cb.GetComponent<BranchSegment>().active = false;
                endPoints.Add(cb.GetComponent<BranchSegment>().getEndPosition());
            }
        }

        //Vector2[,] branchGroup = new Vector2[endPoints.Count, 2];
        List<List<Vector2>> branchGroup = new List<List<Vector2>>();
        for (int i = 0; i < endPoints.Count; i++)
        {
            Vector2 p = endPoints[i];
            bool unique = false;
            float newX;
            do
            {
                newX = p.x + Random.Range(-10.0f, 10.0f);
                unique = true;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Mathf.Abs(newX - branchGroup[j][1].x) < 2)
                    {
                        unique = false;
                    }
                }
            } while (!unique);
            List<Vector2> pl = new List<Vector2>();
            pl.Add(p);
            Vector2 p2 = p;
            p2.y += SectionHeight;
            p2.x = newX;
            pl.Add(p2);
            branchGroup.Add(pl);
        }
        List<List<Vector2>> newBranches = generatePaths(branchGroup, 3);
        List<List<List<Vector2>>> splitBranches = splitGaps(newBranches);
        List<List<List<Vector2>>> lerpBranches = lerp(splitBranches);
        addBranches(lerpBranches[0], true);
        addBranches(lerpBranches[1], false);
    }

    public List<List<List<Vector2>>> lerp(List<List<List<Vector2>>> branches) {
        List<List<List<Vector2>>> return_path = new List<List<List<Vector2>>>();
        foreach (List<List<Vector2>> branch_paths in branches)
        {
            List<List<Vector2>> lerped_paths = new List<List<Vector2>>();
            foreach (List<Vector2> path in branch_paths)
            {
                int curvedLength = path.Count * 3;
                List<Vector2> curvedPoints = new List<Vector2>(curvedLength);
                var t = 0.0f;
                for (int i = 0; i < curvedLength + 1; i++)
                {
                    t = Mathf.InverseLerp(0, curvedLength, i);
                    var points = new List<Vector2>(path);
                    for (int j = path.Count - 1; j > 0; j--)
                    {
                        for (int k = 0; k < j; k++)
                        {
                            points[k] = (1 - t) * points[k] + t * points[k + 1];
                        }
                    }
                    curvedPoints.Add(points[0]);
                }
                lerped_paths.Add(curvedPoints);
            }
            return_path.Add(lerped_paths);
        }
        return return_path;
    }


    public List<List<Vector2>> generatePaths(List<List<Vector2>> branch_paths, int depth)
    {
        if (depth == 0)
        {
            //TODO lerp here
            List<List<Vector2>> return_path = new List<List<Vector2>>();
            for (int i = 0; i<branch_paths.Count; i++)
            {
                var lerped = new List<Vector2>();
                var t = 0.0f;
                for (int j = 0; j<branch_paths[i].Count-1; j++)
                {
                    t = Mathf.InverseLerp(0, (branch_paths[i].Count * 3) - 1, j);
                    lerped.Add(branch_paths[i][j]);
                    lerped.Add((1 - t) * branch_paths[i][j] + t * branch_paths[i][j + 1]);
                }
                lerped.Add(branch_paths[i].Last());
                return_path.Add(lerped);
            }
            return return_path;
        }

        //Vector2[,] genPath = new Vector2[num_bran, num_segs * 2 - 1];
        List<List<Vector2>> genPath = new List<List<Vector2>>();

        //Iterate over each point in bran
        for (int j = 0; j < branch_paths.First().Count-1; j++)
        {
            //Vector2[] startPoints = new Vector2[num_bran];
            //Vector2[] endPoints = new Vector2[num_bran];
            List<Vector2> startPoints = new List<Vector2>();
            List<Vector2> endPoints = new List<Vector2>();
            //Get the start and endpoints for each branch on this segment
            for (int i = 0; i < branch_paths.Count; i++)
            {
                startPoints.Add(branch_paths[i][j]);
                endPoints.Add(branch_paths[i][j + 1]);
                if (j == 0)
                {
                    genPath.Add(new List<Vector2>());
                }
            }
            //Generate the midPoints for each branch together
            List<Vector2> midPoints = genMidPoints(startPoints, endPoints);

            for (int i = 0; i < branch_paths.Count; i++)
            {
                genPath[i].Add(branch_paths[i][j]);
                genPath[i].Add(midPoints[i]);
            }
        }
        //Add in last point
        for (int i = 0; i < branch_paths.Count; i++)
        {
            genPath[i].Add(branch_paths[i].Last());
        }

        return generatePaths(genPath, depth - 1);
    }


    public List<Vector2> genMidPoints(List<Vector2> startPoints, List<Vector2> endPoints)
    {
        List<Vector2> midPoints = new List<Vector2>();

        for (int i = 0; i < startPoints.Count; i++)
        {
            Vector2 start = startPoints[i];
            Vector2 end = endPoints[i];
            var diff = end - start;
            var maxDist = Mathf.Sqrt(diff.magnitude);
            var center = (end + start) / 2f;
            
            var o = Quaternion.Euler(0, 0, 90) * diff.normalized;
            var offset = new Vector2(o.x, o.y);
            //var offset = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * diff.normalized;

            Vector2 midpoint = center + (offset * Random.Range(-maxDist, maxDist));
            midPoints.Add(midpoint);
        }
        return midPoints;
    }


    private void OnDestroy()
    {
        foreach (GameObject br in branches )
        {
            Destroy(br);
        }
    }
    /*
	public void generatePathsOld() {
		List<List<Vector2>> openPaths = new List<List<Vector2>> ();
		List<List<Vector2>> cappedPaths = new List<List<Vector2>> ();
		//Start at endpoints for all ongoing branches
		foreach (GameObject cb in branches) {
			GameObject br = cb.GetComponent<Branch>().segments.Last ();
			if (!br.GetComponent<BranchSegment> ().active) {
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
    */

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

	public void trimBranches(float bot, float top){
        bool grow = false;
		for (int i = branches.Count-1; i>=0; i--) {
			GameObject br = branches [i];
            var end = br.GetComponent<BranchSegment>().getEndPosition().y;
            if (br.GetComponent<BranchSegment>().active && end < top)
                grow = true;
			if (end < bot) {
				branches.RemoveAt (i);
				Destroy(br);
			}
		}

        if (grow)
            generateBranches();
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