using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OreStoneState : State
{
    public float m_OresLeft { get; set; }
    public float m_Hits { get; set; }
}
public class SavableOreStone : Savable 
{
    OreStoneState m_oreStoneState;
    OreStone m_oreStone;
    protected override void InitializeState()
    {
        m_oreStoneState = new OreStoneState();
        m_state = m_oreStoneState;

        m_oreStone = GetComponent<OreStone>();
        m_oreStone.OnPickHit += () => {
            m_oreStoneState.m_OresLeft = m_oreStone.m_OresLeft;
            m_oreStoneState.m_Hits = m_oreStone.m_Hits;
        };
    }
    public override void OnSave()
    {
        base.OnSave();
        m_oreStoneState.m_OresLeft = m_oreStone.m_OresLeft;
        m_oreStoneState.m_Hits = m_oreStone.m_Hits;
    }
    public override void HandleState(State state)
    {
        base.HandleState(state);
        m_oreStoneState = (OreStoneState)state;
        m_state = m_oreStoneState;
        m_oreStone.m_OresLeft = m_oreStoneState.m_OresLeft;
        m_oreStone.m_Hits = m_oreStoneState.m_Hits;
    }
}
