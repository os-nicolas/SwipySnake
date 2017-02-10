using UnityEngine;
using System.Collections;

/* Branches are Lines that curve around through 3d space
 *  for the snake to travel along
 * 
 * 
 * 
 */


public class BranchController : MonoBehaviour {

	public bool isCollidable;

	void Start () {
		isCollidable = true;
	}

	void Update () {
	}

	public Vector3 getNextPos(Vector3 snakePos)
	{
		Vector3 newSnakePos = snakePos;
		newSnakePos.y += .1f;
		return newSnakePos;
	}
}
