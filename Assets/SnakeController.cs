using UnityEngine;
using System.Collections;
using System;

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
    private float       diffY, diffX;
	public bool 		die;
    public Vector3             centerPos;
    WiggleController    wiggleController;
	GameObject			tail;

    void Start ()
    {
        die = false;
        mouseDamper = 50f;
        isJumping = false;
        xVeloc = 0f;
        yVeloc = 0f;
        gravity = .001f;
        collisionCooldown = 0;
        tragLine = this.GetComponent<LineRenderer>();
        centerPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //wiggleController = new WiggleController();
		tail = Instantiate(Resources.Load("SnakeTail")) as GameObject;
		Vector3[] tailPoints = new Vector3[10];
		for (int i = 0; i < 10; i++) {
			tailPoints[i] = new Vector3 (centerPos.x, centerPos.y - i);
		}
		tail.GetComponent<SnakeTail>().points = tailPoints;
    }

    // drawing and input in update
    void Update() {

		if (collisionCooldown > 0) {
			collisionCooldown--;
		}
        

        var mousePos = Input.mousePosition;
		mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		Debug.Log (mousePos.x);

        diffX = (mousePos.x - centerPos.x) / mouseDamper;
        diffY = (mousePos.y - centerPos.y) / mouseDamper;



        if (!isJumping && Input.GetMouseButtonDown(0))
        {
            Jump();
        }

    }

    private void Jump()
    {
        xVeloc = diffX;
        yVeloc = diffY;
        isJumping = true;
        collisionCooldown = 5;
        branchVeloc = 0;

    }

    // we do movement in FixedUpdate
    void FixedUpdate () {
		Camera.main.gameObject.transform.position = new Vector3 (centerPos.x, centerPos.y, -10);

        

        if (isJumping) {
			yVeloc -= gravity;
			transform.position = centerPos;
			centerPos.x += xVeloc;
			centerPos.y += yVeloc;
            //transform.position = wiggleController.UnWiggle(centerPos);
        } else {
            //What was the purpose of these?
			//xVeloc *= .6f;
            //yVeloc *= .9f;
            var branch = currentBranch.GetComponent<Branch_Parent>();
            var branchSpeed = branch.branchSpeed;
            if (branchVeloc < branchSpeed) {
                branchVeloc = ((branchVeloc * 9f) + branchSpeed)/10f;
            }
            var lastp = centerPos;
            centerPos = currentBranch.GetComponent<Branch_Parent>().getNextPosition(centerPos, branchVeloc);
			transform.position = centerPos;
            //transform.position = wiggleController.Wiggle(centerPos, lastp);
   		}
		tail.GetComponent<SnakeTail> ().retraceTail (transform.position);
        drawTrajectory(diffX, diffY);
    }

    private class WiggleController
    {

        Vector3 last = new Vector3(0, 0, 0);
        private readonly float period = 40;
        private readonly float amplitude = .0f;//.5f;
        private float effect = 0;
        private int ticks = 0;

        public WiggleController() {
        }
        
        public Vector3 UnWiggle(Vector3 p)
        {
            effect *= .95f;
            return p + effect * last;
        }

        public Vector3 Wiggle(Vector3 p, Vector3 lastp)
        {
            effect = (10 + effect) / 11f;
            var diff = (p - lastp).normalized;
            var ms = DateTime.Now.Millisecond;
            ticks++;
            var angle = (ticks% period)  * 2 * Mathf.PI/ period;
            var nextMag = Mathf.Sin((float)angle) * amplitude;
            var target = new Vector3(-diff.y * nextMag, diff.x * nextMag,0);
            var lastlast = last;
            last = target;
            return p + effect*((target + lastlast )/ 2f);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {

		if (col.gameObject.name.StartsWith("Obstacle"))
			die = true;
		

		else if (/*isJumping==true &&*/ collisionCooldown == 0) {
			if (col.gameObject.GetComponent<Branch_Parent> () != null &&
			    col.gameObject.GetComponent<Branch_Parent> ().isCollidable == true) {
				//currentBranch = col.gameObject.GetComponent<Branch_Parent>().parent_branch;
				col.gameObject.GetComponent<Branch_Parent> ().isCollidable = false;
				currentBranch.GetComponent<Branch_Parent> ().isCollidable = true;
				currentBranch = col.gameObject;
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

		var firstPoint = new Vector3 (nextX, nextY,1);
		tragLine.SetPosition (0, firstPoint);

		for (int i = 1; i < numPositions; i++) {
			for (int j = 0; j < 4; j++) {
				nextX += xDist;
				nextY += yDist;
				yDist -= gravity;
			}
			var nextDot = new Vector3 (nextX, nextY,1);
			tragLine.SetPosition (i, nextDot);
		}
	}
		
	void OnDestroy() {
		Destroy (tail);
	}
}