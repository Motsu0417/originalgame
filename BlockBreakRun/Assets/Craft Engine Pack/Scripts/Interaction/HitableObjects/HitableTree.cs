using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitableTree : HitableObject {
    public float m_health = 10.0f; 

    public GameObject m_woodSpawns;
    public GameObject m_stickSpawns;

    public GameObject m_woodPrefab;
    public GameObject m_stickPrefab;

    public override void HandleHit(Tool tool)
    {
        ToolTypeAndValue toolTypeAndValuePair;
        if ((toolTypeAndValuePair = new List<ToolTypeAndValue>(tool.m_toolValues).Find((toolval) => toolval.m_Type == ToolType.Axe)) != null)
        {
            if (m_audioSource)
                m_audioSource.Play();
            m_health -= toolTypeAndValuePair.m_HitValue;

            if (m_health <= 0)
            {
                //instantiate logs and sticks
                for (int i = 0; i < m_woodSpawns.transform.childCount; ++i)
                    Instantiate(m_woodPrefab, m_woodSpawns.transform.GetChild(i).position, m_woodSpawns.transform.GetChild(i).rotation);
                for (int i = 0; i < m_stickSpawns.transform.childCount; ++i)
                    Instantiate(m_stickPrefab, m_stickSpawns.transform.GetChild(i).position, m_stickSpawns.transform.GetChild(i).rotation);
                Destroy(gameObject);
            }
            if (OnHit != null)
                OnHit();
        }
    }
    public event Action OnHit;
}
