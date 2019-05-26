using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableObject : MonoBehaviour {
    protected AudioSource m_audioSource;
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    public virtual void HandleHit(Tool tool)
    {

    }
    protected bool AreCompatibleObjectAndTool(Tool tool, ToolType type, out float value)
    {
        value = -1.0f;
        ToolTypeAndValue typeAndVal = new List<ToolTypeAndValue>(tool.m_toolValues).Find((tAV) => tAV.m_Type == type);
        if (typeAndVal == null)
            return false;

        value = typeAndVal.m_HitValue;
        if (m_audioSource)
            m_audioSource.Play();
        return true;
    }
}
