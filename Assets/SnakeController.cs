using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour {
    public float        mouseDamper;
    public bool			isJumping;		
	public GameObject	currentBranch;
	public float		xVeloc;
	public float		yVeloc;
	public float		camHeight;
	public float 		camWidth;
	public float 		gravity;
	public LineRenderer tragLine;		//Draws Jump Tragectory Line
	public float		branchSpeed;
	//public Vector2[] 	tragectory;

	void Start () {
		mouseDamper = 50f;
		isJumping = false;
		xVeloc = 0f;
		yVeloc = 0f;
		gravity = .001f;
		branchSpeed = .1f;
		tragLine 	  = this.GetComponent<LineRenderer>();
		tragLine.SetColors (new Color (1f, 1f, 0f, .75f), new Color (1f, 1f, 0f, 0f));
		tragLine.SetWidth (.15f, .15f);
    }

    // drawing and input in update
    void Update()
    {
        var p = transform.position;

        var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        var diffX = (mousePos.x - p.x) / mouseDamper;
        var diffY = (mousePos.y - p.y) / mouseDamper;

        drawTrajectory(diffX, diffY);

        if (!isJumping && Input.GetMouseButtonDown(0))
        {
            xVeloc = diffX;
            yVeloc = diffY;
            isJumping = true;
        }
        
        Camera.main.gameObject.transform.position = new Vector3(p.x, p.y, -10);
    }
    
    // we do movement in FixedUpdate
    void FixedUpdate () {

        var p = transform.position;

        if (isJumping) {
			yVeloc -= gravity;
			p.x += xVeloc;
			p.y += yVeloc;
		}else {
            if (yVeloc < branchSpeed)
            {
                yVeloc += .04f;
            }
            p.x = currentBranch.GetComponent<Branch_Parent>().getNextPosition(transform.position.y);
			p.y += yVeloc;
		}
		transform.position = p;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log ("Collision with " + col.name);
		if (col.gameObject.GetComponent<Branch_Parent>() != null &&
			col.gameObject.GetComponent<Branch_Parent>().isCollidable == true) {
			Debug.Log ("Collision Succeeded!");
			//currentBranch = col.gameObject.GetComponent<Branch_Parent>().parent_branch;
			col.gameObject.GetComponent<Branch_Parent>().isCollidable = false;
			currentBranch.GetComponent<Branch_Parent> ().isCollidable = true;
			currentBranch = col.gameObject;

			isJumping = false;
		}
	}

	void drawTrajectory(float diffX, float diffY)
	{
        
		tragLine.numPositions = 30;

		var xDist = diffX;
		var yDist = diffY;
		var nextX = transform.position.x;
		var nextY = transform.position.y;

		var firstPoint = new Vector2 (nextX, nextY);
		tragLine.SetPosition (0, firstPoint);

		for (int i = 1; i < tragLine.numPositions; i++) {
			for (int j = 0; j < 4; j++) {
				nextX += xDist;
				nextY += yDist;
				yDist -= gravity;
			}
			var nextDot = new Vector2 (nextX, nextY);
			tragLine.SetPosition (i, nextDot);
		}


	}
		
}
