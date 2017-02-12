using UnityEngine;
using System.Collections;

/// <summary>
/// Branches are Lines that curve around through 3d space
/// for the snake to travel along
/// </summary>
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
		newSnakePos.x = transform.position.x;
		return newSnakePos;
	}
}
