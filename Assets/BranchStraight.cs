using UnityEngine;
using System.Collections;

public class BranchStraight : Branch_Parent {
    
	public override void Init(Vector3 p) {
		Vector2 pos = new Vector2(p.x, p.y);
		player_path = new Vector2[10];
		for (int i = 0; i < 10; i++) {
			player_path [i] = pos;
			pos.y += 1f;
		}
	}
}
