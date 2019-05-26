using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StoneState : State //stone state description
{
    public int m_rocks { get; set; }
}

public class SavableStone : Savable 
{
    StoneState m_stoneState;
    HitableStone m_stone;
    protected override void InitializeState()
    {
        m_stoneState = new StoneState();
        //set reference of state to stone state
        m_state = m_stoneState;
        m_stone = GetComponent<HitableStone>();
        m_stone.OnRockDropped += OnStoneRockDrop;
    }
    public override void OnSave()
    {
        base.OnSave();
        m_stoneState.m_rocks = m_stone.m_rocks;
    }
    public override void HandleState(State state)
    {
        base.HandleState(state);
        m_stoneState = (StoneState)state;
        m_state = m_stoneState;

        m_stone.m_rocks = m_stoneState.m_rocks;
    }
    void OnStoneRockDrop()//refresh state
    {
        m_stoneState.m_rocks = m_stone.m_rocks;
    }
}
