using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HummerScript : ToolScript
{

    // Use this for initialization
    void Start()
    {
        attackPower = 3;
        itemId = "tool_hummer";
        tool = "hummer";
        durability = 100;
        dmgDurability = durability + 0;
    }

}
