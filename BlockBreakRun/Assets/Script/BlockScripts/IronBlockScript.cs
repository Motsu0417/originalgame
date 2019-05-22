
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBlockScript : BlockScript
{

    // Use this for initialization
    void Start()
    {
        blockHp = 250;
        material = "iron";
        itemId = "block_iron";
        aptitude = "pickaxe";
        hardend = 10;
    }
}
