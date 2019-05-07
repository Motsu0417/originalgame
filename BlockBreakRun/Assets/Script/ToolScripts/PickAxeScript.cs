using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeScript : ToolScript {
    

	// Use this for initialization
	void Start () {
        itemId = "tool_pickaxe";
        tool = "pickaxe";
        durability = 100;
        dmgDurability = durability + 0;
	}
}
