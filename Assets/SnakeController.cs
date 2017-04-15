using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour {
    public float        mouseDamper;
    public bool			isJumping;		
	public GameObject	currentBranch;
	public float		xVeloc;
	public float		yVeloc;
    public float        branchVeloc;
    public float		camHeight;
	public float 		camWidth;
	public float 		gravity;
	public LineRenderer tragLine;			//Draws Jump Tragectory Line
	public int			collisionCooldown;	//Cooldown Timer avoids collisions immediately after jumping

	void Start () {
		mouseDamper = 50f;
		isJumping = false;
		xVeloc = 0f;
		yVeloc = 0f;
		gravity = .001f;
		collisionCooldown = 0;
		tragLine 	  = this.GetComponent<LineRenderer>();
    }

    // drawing and input in update
    void Update() {

		if (collisionCooldown > 0) {
			collisionCooldown--;
		}

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
			collisionCooldown = 5;
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
		} else {
            var branch = currentBranch.GetComponent<Branch_Parent>();
            var branchSpeed = branch.branchSpeed;
            if (branchVeloc < branchSpeed) {
                branchVeloc = ((branchVeloc * 9f) + branchSpeed)/10f;
            }
            p = currentBranch.GetComponent<Branch_Parent>().getNextPosition(p, branchVeloc);
		}
		transform.position = p;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (/*isJumping==true &&*/ collisionCooldown == 0) {
			Debug.Log ("Collision with " + col.name);
			if (col.gameObject.GetComponent<Branch_Parent> () != null &&
			    col.gameObject.GetComponent<Branch_Parent> ().isCollidable == true) {
				Debug.Log ("Collision Succeeded! " + transform.position.x + transform.position.y);
				//currentBranch = col.gameObject.GetComponent<Branch_Parent>().parent_branch;
				col.gameObject.GetComponent<Branch_Parent> ().isCollidable = false;
				currentBranch.GetComponent<Branch_Parent> ().isCollidable = true;
				currentBranch = col.gameObject;
				branchVeloc = 0;
				isJumping = false;
				collisionCooldown = 5;
			}
		}
	}

	void drawTrajectory(float diffX, float diffY) {
		var numPositions = 30;
		tragLine.SetVertexCount (numPositions);

		var xDist = diffX;
		var yDist = diffY;
		var nextX = transform.position.x;
		var nextY = transform.position.y;

		var firstPoint = new Vector2 (nextX, nextY);
		tragLine.SetPosition (0, firstPoint);

		for (int i = 1; i < numPositions; i++) {
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
