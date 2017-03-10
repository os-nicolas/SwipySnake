//using UnityEngine;
//using System.Collections;

/// <summary>
/// Branches are Lines that curve around through 3d space
/// for the snake to travel along
/// </summary>
 
/*
public class BranchController : MonoBehaviour {


	public Branch_Parent[] pieces;
	public Vector2 nextPieceLocation;
	public int currentPiece;
	public bool isCollidable;

	void Start () {
		isCollidable = true;
		pieces = new Branch_Parent[3];
		currentPiece = 0;
		for (int i = 0; i < 3; i++) {
			BranchStraight piece = Instantiate(Resources.Load("Branch_Straight")) as BranchStraight;
			piece.transform.position = new Vector3(nextPieceLocation.x, nextPieceLocation.y, 0);
			pieces [i] = piece;
			nextPieceLocation = piece.getEndPosition ();
		}
	}

	void Update () {
		//If a branch goes off the screen, replace it with a new one
	}

	public Vector3 getNextPos(Vector3 snakePos)
	{
		if (pieces [currentPiece].finished == true) {
			currentPiece = (currentPiece + 1) % 3;
		}
		Vector2 pos = pieces [currentPiece].getNextPoint ();

		Vector3 newSnakePos = snakePos;
		newSnakePos.y += (newSnakePos.y + pos.y)/2;
		newSnakePos.x = (newSnakePos.x + pos.x)/2;
		if (newSnakePos.x == pos.x && newSnakePos.y == pos.y) {
			pieces [currentPiece].incrementPoint();
		}
		return newSnakePos;
	}
}
*/
