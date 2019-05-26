using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableStone : HitableObject {

    public int m_rocks = 10;// count of rocks that are in stone
    public float m_hitsPerRock = 10.0f;
    public Transform m_rockSpawn;
    public GameObject m_rockPrefab;

    public event System.Action OnRockDropped;

    float m_hits = 0;
    public override void HandleHit(Tool tool)
    {
        ToolTypeAndValue toolTypeAndValuePair;
        if ((toolTypeAndValuePair = new List<ToolTypeAndValue>(tool.m_toolValues).Find((toolval) => toolval.m_Type == ToolType.Hammer)) != null)
        {
            if (m_audioSource)
                m_audioSource.Play();
            m_hits += toolTypeAndValuePair.m_HitValue;

            if (m_hits >= m_hitsPerRock)
            {
                if (m_rocks == 2)//last hit -> instantiate 2 rocks and destroy self
                {
                    Instantiate(m_rockPrefab, m_rockSpawn.position, m_rockSpawn.rotation);
                    Instantiate(m_rockPrefab, m_rockSpawn.position, m_rockSpawn.rotation);
                    Destroy(gameObject);
                    return;
                }
                Instantiate(m_rockPrefab, m_rockSpawn.position, m_rockSpawn.rotation);
                m_hits = 0f;
                --m_rocks;
            }
            if (OnRockDropped != null)
                OnRockDropped();
        }
    }
}
