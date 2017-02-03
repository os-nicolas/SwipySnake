﻿using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour {

	// Use this for initialization
	public bool isJumping;
	public int currentBranch; //0-left 1-center 2-right
	public float xVeloc;
	public float yVeloc;
	public float camHeight;
	public float camWidth;
	public LineRenderer tragLine;
	public float gravity;
	public Vector2[] tragectory;

	void Start () {
		currentBranch = 1;
		isJumping = false;
		xVeloc = 0f;
		yVeloc = 0f;
		gravity = .001f;
		camHeight = Camera.main.orthographicSize;
		camWidth = Camera.main.aspect * camHeight;
		tragLine = this.GetComponent<LineRenderer> ();
		tragLine.SetColors (Color.yellow, Color.red);
		tragLine.SetWidth (.15f, .15f);
	}
		
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log ("xVeloc: " + xVeloc);
		//Debug.Log ("yVeloc: " + yVeloc);
		Vector3 p = transform.position;
		if (isJumping == true) {
			yVeloc -= gravity;
			p.x += xVeloc;
			p.y += yVeloc;
			Vector3 screenPoint = Camera.main.gameObject.transform.position;
			if (screenPoint.x < 0 || screenPoint.x > camHeight) {
				p.x = 0;
				xVeloc = 0;
			}
			if (screenPoint.y < 0 || screenPoint.y > camWidth) {
				p.y = 0;
			}
			transform.position = p;
		}
		else {
			Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			mousePos = Camera.main.ScreenToWorldPoint (mousePos);

			float diffX = (mousePos.x - p.x)/50;
			float diffY = (mousePos.y - p.y)/50;
			drawTrajectory (mousePos, diffX, diffY);
			//Debug.Log ("diffY : " + diffY);
			//Debug.Log ("diffX : " + diffX);
			if (Input.GetMouseButtonDown(0)) {
				xVeloc = diffX;
				yVeloc = diffY;
				isJumping = true;
				tragLine.SetPosition(0, Vector3.zero);
				tragLine.SetPosition(1, Vector3.zero);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log ("Collision with " + col.gameObject.name);
		if (col.gameObject.name == "branch_" + ((currentBranch + 1) % 3).ToString()) {
			currentBranch = (currentBranch + 1) % 3;
			xVeloc = 0;
			yVeloc = 0;
			isJumping = false;
		} else if (col.gameObject.name == "branch_" + ((currentBranch + 2) % 3).ToString()) {
			currentBranch = (currentBranch + 2) % 3;
			xVeloc = 0;
			yVeloc = 0;
			isJumping = false; 
		}
	}

	void drawTrajectory(Vector2 mousePos, float diffX, float diffY)
	{
		
		Vector2 mPos = Camera.main.ScreenToWorldPoint (mousePos);

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
