using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenOreContainer : OvenContainer {
    public OreType? m_OreType { get; set; }
    protected override bool AreCompatibleObjectInHandsAndContainerType()
    {
        try
        {
            OreType type = ((Ore)m_objectInHands).m_type;
            return base.AreCompatibleObjectInHandsAndContainerType() && m_OreType == type;
        }
        catch
        {
            return false;
        }
    }
}
