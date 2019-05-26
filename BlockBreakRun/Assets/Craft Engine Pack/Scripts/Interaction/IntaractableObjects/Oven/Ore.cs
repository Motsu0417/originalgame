using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum OreType
{
    Iron,
    Gold,
    Silver,
    Copper
}

public class Ore : ContainerElement 
{
    public OreType m_type;
    public float m_necessaryFuelValue;
}
