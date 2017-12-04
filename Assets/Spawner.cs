using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// 6 7 8
// 3 4 5
// 0 1 2

/*NOTE: 
 */

//FIXME: No need for the tail to reinitialize, just move and reset it
public class Spawner : MonoBehaviour {

	public GameObject player;
	public GameObject fire;
	public GameObject branchCtrl;

	// Use this for initialization
	void Start () {
		

		//Vector3[,] branchGroup = new Vector3[3, 2];
		player = Instantiate(Resources.Load("Snake")) as GameObject;
		fire = Instantiate (Resources.Load ("FireLine")) as GameObject;
		branchCtrl = Instantiate (Resources.Load ("BranchCtrl")) as GameObject;
		player.GetComponent<SnakeController> ().currentBranch = branchCtrl.GetComponent<BranchController> ().branches [1];
    }
	
	// Update is called once per frame
	void Update () {
    
        var screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
        var screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));

        var screenWidth = screenTopRight.x - screenBottomLeft.x;
        var screenHeight = screenTopRight.y - screenBottomLeft.y;

        if (player.GetComponent<SnakeController> ().die == true) {
			ResetGame ();
		}
        branchCtrl.GetComponent<BranchController>().trimBranches(screenBottomLeft.y, screenTopRight.y + 3);
        branchCtrl.GetComponent<BranchController>().wrapBranches(screenBottomLeft.x, screenTopRight.x);
        Vector3 firepos = fire.transform.position;

        if (firepos.y < screenBottomLeft.y + .5f)
        {
            firepos.y = screenBottomLeft.y + .5f;
        }
        else
        {
            firepos.y += .02f * TimeSlower.TimeScale;
        }
        fire.transform.position = firepos;
		if (player.transform.position.y < firepos.y) {
			ResetGame ();
		}
	}

	void ResetGame() {
		Destroy (player);
		Destroy (fire);
		Destroy (branchCtrl);
		Start ();
	}
}