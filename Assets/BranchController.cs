using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 	BranchController handles the generation and deletion of branch objects

/// </summary>

public class BranchController : MonoBehaviour {

    public List<GameObject> branches;
    //public GameObject[] obstacles;
    private int curveHeight = 5;
    private int sectionCurves = 4;
    private float SectionHeight = 20.0f;
    public float screenWidth;

    void Awake() {
        var screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
        var screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));

        screenWidth = screenTopRight.x - screenBottomLeft.x;


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
    public List<List<int>> getIntersects(List<Vector2> a, List<Vector2> b, float threshold = 0.5f)
    {
        List<int> Aintersects = new List<int>();
        List<int> Bintersects = new List<int>();
        List<List<int>> intersects = new List<List<int>>();
        int x = 1;
        int y = 1;
        //char aSide = 'N'; //Start in Neutral position
        Vector2 aPos = a[1];
        Vector2 bPos = b[1];
        //Check only the points where the two sets begin to overlap on y coordinate
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

        //Find overlap points on x coordinate
        float lastA;
        float lastB;
        while (x < a.Count && y < b.Count)
        {
            lastB = b[y - 1].x;
            bPos = b[y];
            for (int i=-1; i<2; i++)
            {
                lastA = a[x - 1].x + (i*screenWidth);
                aPos = a[x];
                aPos.x += i * screenWidth;

                //Step 1: Check the new position of the branches to find overlaps
                //Case 1: the two branches meet, always consider as an overlap
                if (Mathf.Abs(aPos.x - bPos.x) < threshold)
                {
                    if (!Aintersects.Contains(x))
                    {
                        Aintersects.Add(x);
                    }
                    if (!Bintersects.Contains(y))
                    {
                        Bintersects.Add(y);
                    }
                    break;
                }
                //Case 2: Cross right to left
                else if (aPos.x < bPos.x + threshold && lastA > lastB - threshold)
                {
                    if (!Aintersects.Contains(x))
                    {
                        Aintersects.Add(x);
                    }
                    if (!Bintersects.Contains(y))
                    {
                        Bintersects.Add(y);
                    }
                    break;
                }
                else if (aPos.x > bPos.x - threshold && lastA < lastB + threshold)
                {
                    if (!Aintersects.Contains(x))
                    {
                        Aintersects.Add(x);
                    }
                    if (!Bintersects.Contains(y))
                    {
                        Bintersects.Add(y);
                    }
                    break;
                }
            }
            lastA = aPos.x;
            lastB = bPos.x;

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
    public List<List<List<Vector2>>> splitGaps(List<List<Vector2>> branch_paths)
    {
        //gaps holds the points to split on for each branch path
        List<List<int>> gaps = new List<List<int>>();
        for (int i = 0; i < branch_paths.Count; i++)
        {
            gaps.Add(new List<int>());
        }

        //Add all intersect points for each combination of branch paths
        for (int i = 0; i < branch_paths.Count; i++)
        {
            for (int j = i + 1; j < branch_paths.Count; j++)
            {
                var g = getIntersects(branch_paths[i], branch_paths[j]);
                gaps[i] = gaps[i].Union<int>(g[0]).ToList<int>();
                gaps[j] = gaps[j].Union<int>(g[1]).ToList<int>();
                /*
                foreach (int x in g[0])
                {
                    if (!gaps[i].Contains(x))
                        gaps[i].Add(x);
                }
                foreach (int x in g[1])
                {
                    if (!gaps[j].Contains(x))
                        gaps[j].Add(x);
                }
                */
            }
        }

        List<List<List<Vector2>>> newBranches = new List<List<List<Vector2>>>();
        newBranches.Add(new List<List<Vector2>>()); //active branches
        newBranches.Add(new List<List<Vector2>>()); //inactive branches

        for (int i = 0; i < gaps.Count; i++)
        {
            //Iterate through gaps and create inactive branches before each
            int start = 0;
            for (int j = 0; j < gaps[i].Count - 1; j++)
            {
                if (gaps[i][j] - start > 1)
                    newBranches[1].Add(branch_paths[i].GetRange(start, gaps[i][j] - (start+1)));
                start = gaps[i][j] + 1;
            }
            //Add the remainder as an active branch
            if (branch_paths[i].Count == start)
                newBranches[0].Add(new List<Vector2> { branch_paths[i].Last() });
            else
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
                    if (Mathf.Abs(newX - branchGroup[j][1].x) < 2
                        || Mathf.Abs(newX - (branchGroup[j][1].x + screenWidth)) < 2
                        || Mathf.Abs(newX - (branchGroup[j][1].x - screenWidth)) < 2)
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
        
        List<List<Vector2>> genPath = new List<List<Vector2>>();

        //Iterate over each point in bran
        for (int j = 0; j < branch_paths.First().Count-1; j++)
        {
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


    public void wrapBranches(float left, float right)
    {
        var wrapDist = right - left;
        int x = 0;
        //Remove branches that have gone left or right off the screen
        for (int i = branches.Count - 1; i >= 0; i--)
        {
            GameObject br = branches[i];
            
            if (br.GetComponent<BranchSegment>().active)
            {
                x++;
            }

            if (br.GetComponent<BranchSegment>().wrapCopy != null && (br.GetComponent<BranchSegment>().rightmost < left - 3 || br.GetComponent<BranchSegment>().leftmost > right + 3))
            {
                if (br.GetComponent<BranchSegment>().wrapCopy == null)
                    Debug.LogError("About to remove a branch without a copy");
                if (br.GetComponent<BranchSegment>().active)
                {
                    br.GetComponent<BranchSegment>().wrapCopy.GetComponent<BranchSegment>().active = true;
                }
                branches.RemoveAt(i);
                Destroy(br);
            }
        }

        if (x != 3)
            Debug.LogError("Not enough branch ends!");

        //Wrap branches on the edge of the screen by copying them to the other side
        List<GameObject> newBranches = new List<GameObject>();
        foreach (GameObject branch in branches)
        {
            if (branch.GetComponent<BranchSegment>().wrapCopy == null)
            {
                List<Vector2> wPath = new List<Vector2>();
                if (branch.GetComponent<BranchSegment>().leftmost <= left)
                {
                    wPath = branch.GetComponent<BranchSegment>().getWrapPath(wrapDist);
                }
                else if (branch.GetComponent<BranchSegment>().rightmost >= right)
                {
                    wPath = branch.GetComponent<BranchSegment>().getWrapPath(-wrapDist);
                }

                if (wPath.Count != 0)
                {
                    GameObject br = GameObject.Instantiate(Resources.Load("BranchSegment")) as GameObject;
                    br.GetComponent<BranchSegment>().setPath(wPath, false);
                    br.GetComponent<BranchSegment>().wrapCopy = branch;
                    newBranches.Add(br);
                    branch.GetComponent<BranchSegment>().wrapCopy = br;
                }
            }
        }
        branches.AddRange(newBranches);

    }

}
