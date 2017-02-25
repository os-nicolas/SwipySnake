using UnityEngine;
using System.Collections;

public class BranchStraight : Branch_Parent {

	// Use this for initialization
	public BranchStraight () {
		var p = transform.position;
		Vector2 pos = new Vector2(p.x, p.y);
		player_path = new Vector2[10];
		for (int i = 0; i < 10; i++) {
			player_path [i] = pos;
			pos.x += 10f;
		}
	}
}
