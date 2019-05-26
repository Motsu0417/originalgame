using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public MenuController m_menuController;
    Inventory m_inventory;
    public GameObject m_chestCellsContainer;
    public CraftingDesk m_currentCraftingDesk { get; set; }
    List<CellRenderer> m_cellRenderers;
    public Chest m_currentChest
    {
        set
        {
            for(int i = 0; i < m_cellRenderers.Count; ++i)
            {
                m_cellRenderers[i].m_cell = value.m_cells[i];
                value.m_cells[i].m_renderer = m_cellRenderers[i];
                value.m_cells[i].m_view = m_inventory.m_view;
                m_inventory.m_view.CellItemChangedHandler(value.m_cells[i]);
                m_inventory.m_view.CellCountChangedHandler(value.m_cells[i]);
            }
        }
    }
    public int m_ChestCellsCount
    {
        get
        {
            return m_chestCellsContainer.transform.childCount;
        }
    }
    void Awake()
    {
        m_inventory = GameObject.Find("Player").GetComponent<Inventory>();
        m_cellRenderers = new List<CellRenderer>();
        foreach (CellRenderer rend in m_chestCellsContainer.GetComponentsInChildren(typeof(CellRenderer), true))
            m_cellRenderers.Add(rend);
    }
    public void F_SetBlank(Blank blank)
    {
        m_currentCraftingDesk.m_Blank = blank;
        m_inventory.TakeAwaySelectedTool(1);
    }
    public MenuController GetMenuController()
    {
        return m_menuController;
    }
}
