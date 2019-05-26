using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableLog : HitableObject {

    public float m_health = 6.0f;//like a health of log
    public uint m_stickCount = 5;//drop count
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
                for (int i = 0; i < m_stickCount; ++i) // instantiate sticks
                {
                    Vector2 randomCircle = Random.insideUnitCircle;
                    Instantiate(m_stickPrefab, transform.position + new Vector3(randomCircle.x, 0.0f, randomCircle.y), transform.rotation);
                }
                Destroy(gameObject);
            }
        }
    }
}
