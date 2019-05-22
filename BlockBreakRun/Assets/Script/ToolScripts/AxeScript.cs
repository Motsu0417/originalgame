using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : ToolScript {

	// Use this for initialization
	void Start () {
        attackPower = 4;
        itemId = "tool_axe";
        tool = "axe";
        durability = 100;
        dmgDurability = durability + 0;
    }
}
