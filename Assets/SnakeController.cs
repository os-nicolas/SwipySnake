using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour {

	public bool			isJumping;		
	public GameObject	currentBranch;
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
		//camHeight 	  = Camera.main.orthographicSize;
		//camWidth 	  = Camera.main.aspect * camHeight;
		tragLine 	  = this.GetComponent<LineRenderer>();
		tragLine.SetColors (new Color(1f, 1f, 0f, .75f), new Color(1f,1f,0f,0f));
		tragLine.SetWidth (.15f, .15f);
	}
		
	// Update is called once per frame
	void FixedUpdate () {
        var p = transform.position;

        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        float diffX = (mousePos.x - p.x) / 50;
        float diffY = (mousePos.y - p.y) / 50;
        
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
				p = currentBranch.GetComponent<BranchController> ().getNextPos(transform.position);
			}
		}
		transform.position = p;
		Camera.main.gameObject.transform.position = new Vector3(p.x, p.y, -10);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log ("Collision with " + col.name);
		if (col.gameObject.GetComponent<BranchController>() != null && col.gameObject.GetComponent<BranchController>().isCollidable == true) {
			Debug.Log ("Collision2222!");
			currentBranch = col.gameObject;
			isJumping = false;
		}
	}

	void drawTrajectory(Vector2 mousePos, float diffX, float diffY)
	{

		int v = 30; // number of vertices

		tragLine.SetVertexCount (v);

		float xDist = diffX;
		float yDist = diffY;
		float nextX = transform.position.x;
		float nextY = transform.position.y;

		Vector2 firstPoint = new Vector2 (nextX, nextY);
		tragLine.SetPosition (0, firstPoint);

		for (int i = 1; i < v; i++) {
			for (int j = 0; j < 4; j++) {
				nextX += xDist;
				nextY += yDist;
				yDist -= gravity;
			}
			Vector2 nextDot = new Vector2 (nextX, nextY);
			tragLine.SetPosition (i, nextDot);
			//trag = Vector2.Lerp (trag, nextDot, t);
		}


	}
		
}
