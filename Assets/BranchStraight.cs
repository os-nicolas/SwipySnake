using UnityEngine;
using System.Collections;

public class BranchStraight : Branch_Parent {
<<<<<<< HEAD

	// Use this for initialization
	public BranchStraight () {
		var p = transform.position;
=======
    
	public override void Init(Vector3 p) {
>>>>>>> b88915c488ea0085d16e8aab1d62e085b9e6a5eb
		Vector2 pos = new Vector2(p.x, p.y);
		player_path = new Vector2[10];
		for (int i = 0; i < 10; i++) {
			player_path [i] = pos;
<<<<<<< HEAD
			pos.y += 3f;
=======
			pos.y += 1f;
>>>>>>> b88915c488ea0085d16e8aab1d62e085b9e6a5eb
		}
	}
}
