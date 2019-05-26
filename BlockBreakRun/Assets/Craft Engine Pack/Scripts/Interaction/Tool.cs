using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum ToolType
{
    Axe,
    Hammer,
    Knife,
    Pick
}

[Serializable]
public class ToolTypeAndValue
{
    public ToolType m_Type;
    public float m_HitValue;
}

public class Tool : MonoBehaviour {
    public AccessViaRayCast m_eyes { get; set; }
    public ToolTypeAndValue[] m_toolValues;
    public virtual void Action()
    {
        if (m_eyes.m_objectToHit)
            m_eyes.m_objectToHit.HandleHit(GetComponent<Tool>());
    }
}
