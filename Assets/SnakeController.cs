using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour {
    public float        mouseDamper = 50f;
    public bool			isJumping;		
	public BranchController	currentBranch;
	public float		xVeloc;
	public float		yVeloc;
	public float		camHeight;
	public float 		camWidth;
	public float 		gravity;
	public LineRenderer tragLine;		//Draws Jump Tragectory Line
	//public Vector2[] 	tragectory;

	void Start () {
		isJumping 	  = false;
		xVeloc 		  = 0f;
		yVeloc 		  = 0f;
		gravity		  = .001f;
		//camHeight	  = Camera.main.orthographicSize;
		//camWidth 	  = Camera.main.aspect * camHeight;
		tragLine 	  = this.GetComponent<LineRenderer>();
		tragLine.SetColors (new Color(1f, 1f, 0f, .75f), new Color(1f,1f,0f,0f));
		tragLine.SetWidth (.15f, .15f);
	}
		
	// Update is called once per frame
	void FixedUpdate () {
        var p = transform.position;

        var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        var diffX = (mousePos.x - p.x) / mouseDamper;
        var diffY = (mousePos.y - p.y) / mouseDamper;
        
        drawTrajectory(mousePos, diffX, diffY);

		if (isJumping == true) {
			yVeloc -= gravity;
			p.x += xVeloc;
			p.y += yVeloc;
		}else {
			if (Input.GetMouseButtonDown (0)) {
				xVeloc = diffX;
				yVeloc = diffY;
				isJumping = true;
				tragLine.SetPosition (0, Vector3.zero);
				tragLine.SetPosition (1, Vector3.zero);
			} else {
				p = currentBranch.getNextPos(transform.position);
			}
		}
		transform.position = p;
		Camera.main.gameObject.transform.position = new Vector3(p.x, p.y, -10);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log ("Collision with " + col.name);
		if (col.name.StartsWith("Branch") &&
			//col.gameObject.GetComponent<Branch_Parent>() != null &&
			col.gameObject.GetComponent<Branch_Parent>().parent_branch.isCollidable == true) {
			Debug.Log ("Collision!");
			currentBranch = col.gameObject.GetComponent<Branch_Parent>().parent_branch;
			currentBranch.currentPiece = col.gameObject.GetComponent<Branch_Parent>().myIndex;
			isJumping = false;
		}
	}

	void drawTrajectory(Vector2 mousePos, float diffX, float diffY)
	{

		int v = 30; // number of vertices

		tragLine.SetVertexCount (v);

		var xDist = diffX;
		var yDist = diffY;
		var nextX = transform.position.x;
		var nextY = transform.position.y;

		var firstPoint = new Vector2 (nextX, nextY);
		tragLine.SetPosition (0, firstPoint);

		for (int i = 1; i < v; i++) {
			for (int j = 0; j < 4; j++) {
				nextX += xDist;
				nextY += yDist;
				yDist -= gravity;
			}
			var nextDot = new Vector2 (nextX, nextY);
			tragLine.SetPosition (i, nextDot);
			//trag = Vector2.Lerp (trag, nextDot, t);
		}


	}
		
}
