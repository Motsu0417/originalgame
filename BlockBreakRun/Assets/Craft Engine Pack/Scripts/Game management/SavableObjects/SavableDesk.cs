using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeskState : State
{
    public string m_blankName { get; set; }
    public float m_health { get; set; }
}

public class SavableDesk : Savable 
{
    DeskState m_deskState;
    CraftingDesk m_desk;
    protected override void InitializeState()
    {
        base.InitializeState();
        m_desk = GetComponent<CraftingDesk>();
        m_deskState = new DeskState();
        m_state = m_deskState;
    }
    public override void OnSave()
    {
        base.OnSave();
        m_deskState.m_blankName = m_desk.m_Blank ? m_desk.m_Blank.gameObject.name : "";
        m_deskState.m_health = m_desk.m_craftingDeskWorkplace.m_health;
    }
    public override void HandleState(State state)
    {
        base.HandleState(state);
        m_deskState = (DeskState)state;
        m_state = m_deskState;
        m_desk.m_Blank = Resources.Load<Blank>("Items/" + m_deskState.m_blankName);
        m_desk.m_craftingDeskWorkplace.m_health = m_deskState.m_health;
    }
}
