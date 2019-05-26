using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnvilState : State
{
    public string m_LayoutString { get; set; }
    public float m_ForgeProgress { get; set; }
}

public class SavableAnvil : Savable 
{
    AnvilState m_anvilState;
    InteractableAnvil m_anvil;
    protected override void InitializeState()
    {
        base.InitializeState();
        m_anvilState = new AnvilState();
        m_state = m_anvilState;
        m_anvil = GetComponent<InteractableAnvil>();
    }
    public override void OnSave()
    {
        base.OnSave();
        m_anvilState.m_ForgeProgress = m_anvil.m_hitableAnvil.m_forgeProgress;
        m_anvilState.m_LayoutString = m_anvil.m_LayoutDescription ? m_anvil.m_LayoutDescription.gameObject.name : "";
    }
    public override void HandleState(State state)
    {
        base.HandleState(state);
        m_anvilState = (AnvilState)state;
        m_state = m_anvilState;
        m_anvil.m_LayoutDescription = Resources.Load<Layout>("Items/" + m_anvilState.m_LayoutString);
        m_anvil.m_hitableAnvil.m_forgeProgress = m_anvilState.m_ForgeProgress;
    }
}
