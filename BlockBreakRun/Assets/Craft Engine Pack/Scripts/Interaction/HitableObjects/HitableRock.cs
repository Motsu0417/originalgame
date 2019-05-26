using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableRock : HitableObject {
    public float m_health = 2.0f;
    public GameObject m_fragmentPrefab;
    public override void HandleHit(Tool tool)
    {
        ToolTypeAndValue toolTypeAndValuePair;
        if((toolTypeAndValuePair = new List<ToolTypeAndValue>(tool.m_toolValues).Find((toolValue)=>toolValue.m_Type == ToolType.Hammer)) != null)
        {
            if (m_audioSource)
                m_audioSource.Play();
            m_health -= toolTypeAndValuePair.m_HitValue;

            if (m_health <= 0)
            {// instantiate two stone fragments and destroy self
                Instantiate(m_fragmentPrefab, transform.position, transform.rotation);
                Instantiate(m_fragmentPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
