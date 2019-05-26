using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreStone : HitableObject 
{
    public float m_OresLeft { get; set; }
    public float m_Hits { get; set; }
    public event System.Action OnPickHit;

    public float m_hitsToEasyMineOre;
    public float m_easyMineOreCount;
    public float m_hitsToDifficultMineOre;
    public Transform m_dropPoint;
    public GameObject m_orePrefab;
    void Awake()
    {
        m_OresLeft = m_easyMineOreCount;
        m_Hits = 0;
    }
    public override void HandleHit(Tool tool)
    {
        float val;
        if (AreCompatibleObjectAndTool(tool, ToolType.Pick, out val))
        {
            m_Hits += val;

            if (m_OresLeft > 0f)
            {
                if(m_Hits >= m_hitsToEasyMineOre)
                {
                    --m_OresLeft;
                    Instantiate(m_orePrefab, m_dropPoint.position, m_dropPoint.rotation);
                    m_Hits = 0;
                }
            }
            else if (m_Hits >= m_hitsToDifficultMineOre)
            {
                Instantiate(m_orePrefab, m_dropPoint.position, m_dropPoint.rotation);
                m_Hits = 0;
            }
            if (OnPickHit != null)
                OnPickHit();
        }
    }
}
