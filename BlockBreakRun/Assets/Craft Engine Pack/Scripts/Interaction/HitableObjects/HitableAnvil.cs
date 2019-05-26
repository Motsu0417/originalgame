using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableAnvil : HitableObject 
{
    Layout layout;
    public float m_forgeProgress { get; set; }
    public GameObject m_LayoutObject
    {
        get;
        set;
    }
    public Layout m_LayoutDescription
    {
        get
        {
            return layout;
        }
        set
        {
            m_forgeProgress = value.m_forgeTime;
            layout = value;
        }
    }

    public override void HandleHit(Tool tool)
    {
        if (!m_LayoutObject)
            return;
        float val;
        if (AreCompatibleObjectAndTool(tool, ToolType.Hammer, out val))
        {
            m_forgeProgress -= val;
            if(m_forgeProgress <= 0)
            {
                Instantiate(m_LayoutDescription.m_futureObject.m_prefab, m_LayoutObject.transform.position, m_LayoutObject.transform.rotation);
                Destroy(m_LayoutObject);
            }
        }
    }
}
