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

	public void Init (Vector2 p) {
        nextPieceLocation = p;
        isCollidable = true;
		pieces = new Branch_Parent[3];
		currentPiece = 0;
		for (int i = 0; i < 3; i++) {
<<<<<<< HEAD
			BranchStraight piece = Instantiate(Resources.Load("Branch_Straight")) as BranchStraight;
			piece.transform.position = new Vector3(nextPieceLocation.x, nextPieceLocation.y, 0);
=======
            var BranchStraightGameObject = Instantiate(Resources.Load("Branch_Straight")) as GameObject;
            BranchStraight piece = BranchStraightGameObject.GetComponent<BranchStraight>();
			piece.Init(new Vector3(nextPieceLocation.x, nextPieceLocation.y, 0));
			piece.parent_branch = this;
			piece.myIndex = i;
>>>>>>> b88915c488ea0085d16e8aab1d62e085b9e6a5eb
			pieces [i] = piece;
			nextPieceLocation = piece.getEndPosition ();
		}
	}
    
	public Vector3 getNextPos(Vector3 snakePos)
	{
		if (pieces [currentPiece].finished == true) {
			currentPiece = (currentPiece + 1) % 3;
		}
		Vector2 pos = pieces [currentPiece].getNextPoint ();

		Vector3 newSnakePos = snakePos;
		newSnakePos.y = (newSnakePos.y + pos.y)/2;
		newSnakePos.x = (newSnakePos.x + pos.x)/2;
		if (newSnakePos.x == pos.x && newSnakePos.y == pos.y) {
			pieces [currentPiece].incrementPoint();
		}
		return newSnakePos;
	}
}
*/
