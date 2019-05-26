using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestState : State
{
    public List<CellState> m_chestCells { get; set; }
}

public class SavableChest : Savable 
{
    Chest m_chest;
    ChestState m_chestState;
    protected override void InitializeState()
    {
        base.InitializeState();
        m_chest = GetComponent<Chest>();
        m_chestState = new ChestState { m_chestCells = new List<CellState>() };
        m_state = m_chestState;
    }
    public override void OnSave()
    {
        base.OnSave();
        m_chestState.m_chestCells = new List<CellState>();
        foreach (Cell c in m_chest.m_cells)
            m_chestState.m_chestCells.Add(new CellState
            {
                m_count = c.m_count,
                m_itemDescriptionPrefabName = c.m_item ? c.m_item.gameObject.name : ""
            });
    }
    public override void HandleState(State state)
    {
        base.HandleState(state);
        m_chestState = (ChestState)state;
        m_state = m_chestState;
        for(int i = 0; i < m_chestState.m_chestCells.Count; ++i)
        {
            m_chest.m_cells[i].m_count = m_chestState.m_chestCells[i].m_count;
            m_chest.m_cells[i].m_item = Resources.Load<ItemDescription>("Items/" + m_chestState.m_chestCells[i].m_itemDescriptionPrefabName);
        }
    }
}
