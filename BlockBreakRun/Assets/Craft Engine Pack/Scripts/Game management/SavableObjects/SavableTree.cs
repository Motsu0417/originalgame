using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeState : State
{
    public float m_hits { get; set; }
    public TreeState() { }
    public TreeState(State st) : base(st) { }
}

public class SavableTree : Savable {
    TreeState m_treeState;
    HitableTree m_tree;

    public override void OnSave()
    {
        base.OnSave();
        m_treeState.m_hits = m_tree.m_health;
    }
    protected override void InitializeState()
    {
        m_treeState = new TreeState();
        m_state = m_treeState;
        m_tree = GetComponent<HitableTree>();
        m_treeState.m_hits = m_tree.m_health;
        m_tree.OnHit += OnTreeHit;
    }
    public override void HandleState(State state)
    {
        base.HandleState(state);
        m_treeState = (TreeState)state;
        m_state = m_treeState;

        m_tree.m_health = m_treeState.m_hits;
    }
    void OnTreeHit()
    {
        m_treeState.m_hits = m_tree.m_health;
    }
}
