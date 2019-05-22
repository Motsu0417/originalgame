using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : ToolScript {

    // Use this for initialization
    void Start()
    {
        itemId = "hand";
        tool = "hand";
        durability = 100000000;
        dmgDurability = durability + 0;
        attackPower = 5;
    }
}
