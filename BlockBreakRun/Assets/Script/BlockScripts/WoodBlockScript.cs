using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBlockScript : BlockScript {

	// Use this for initialization
	void Start () {
        blockHp = 100;
        material = "wood";
        itemId = "block_wood";
        hardend = 4;
        aptitude = "axe";
	}
}
