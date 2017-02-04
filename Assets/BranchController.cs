using UnityEngine;
using System.Collections;

/* Branches are Lines that curve around through 3d space
 *  for the snake to travel along
 * 
 * 
 * 
 */


public class BranchController : MonoBehaviour {

	Queue positions;
	LineRenderer line;
	int branchNum;
	int vertices;

	void Start () {
		//Determine which branch is calling the script
		if (gameObject.name == "branch_1") {
			branchNum = 1;
		} else if (gameObject.name == "branch_2") {
			branchNum = 2;
		} else if (gameObject.name == "branch_3") {
			branchNum = 3;
		}

		//Initialize LineRender to draw branch as we go
		line = this.GetComponent<LineRenderer> ();
		line.SetColors (Color.green, Color.green);
		line.SetWidth (1.5f, 1.5f);



	}

	void Update () {
		DrawBranch ();

	}

	void DrawBranch() {
		int vertices = 50;
		line.SetVertexCount (vertices);
		for (int i = 0; i < vertices; i++) {
			line.SetPosition (i, positions [i]);
		}
	}
}
