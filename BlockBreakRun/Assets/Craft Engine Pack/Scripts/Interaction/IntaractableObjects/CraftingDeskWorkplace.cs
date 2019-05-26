using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDeskWorkplace : HitableObject 
{
    Blank blank;
    public CraftingDesk m_craftingDesk;
    public float m_health { get; set; }
    GameObject m_log;
    public Blank m_Blank
    {
        get
        {
            return blank;
        }
        set
        {
            blank = value;
            if (blank == null)
                return;

            m_health = blank.m_cuttingTime;
            m_log = Instantiate(m_craftingDesk.m_logDescription.m_prefab, m_craftingDesk.m_logTransform.position, m_craftingDesk.m_logTransform.rotation);
            Savable sav = m_log.GetComponent<Savable>();
            if (sav)
                sav.m_isFromFileOrInventory = true;
            Rigidbody rb = m_log.GetComponent<Rigidbody>();
            if (rb)
                Destroy(rb);
            foreach (Collider c in m_log.GetComponentsInChildren<Collider>())
                Destroy(c);
        }
    }
    public override void HandleHit(Tool tool)
    {
        float value;
        if(base.AreCompatibleObjectAndTool(tool, ToolType.Knife, out value))
        {
            m_health -= value;
            if(m_health <= 0)
            {
                Destroy(m_log);
                Instantiate(m_Blank.m_prefab, m_craftingDesk.m_logTransform.position, Quaternion.EulerAngles(new Vector3()));
                m_craftingDesk.m_Blank = null;
            }
        }
    }
}
