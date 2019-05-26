using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject 
{
    public List<Cell> m_cells { get; private set; }
    MenuController m_menu;
    GameManager m_gameManager;
    void Awake()
    {
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_menu = m_gameManager.GetMenuController();
        m_cells = new List<Cell>();

        for (int i = 0; i < m_gameManager.m_ChestCellsCount; ++i)
            m_cells.Add(new Cell());
    }
    public override void Interact()
    {
        m_gameManager.m_currentChest = GetComponent<Chest>();
        m_menu.InvokeChestMenu();
    }
}
