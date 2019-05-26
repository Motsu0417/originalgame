using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDesk : InteractableObject {
    public Transform m_logTransform;
    public ItemDescription m_logDescription;
    public GameObject m_workplace;
    public CraftingDeskWorkplace m_craftingDeskWorkplace { get; private set; }
    public Blank m_Blank
    {
        get
        {
            return m_craftingDeskWorkplace.m_Blank;
        }
        set
        {
            if (value != null)
                m_workplace.SetActive(true);
            else
                m_workplace.SetActive(false);

            m_craftingDeskWorkplace.m_Blank = value;
        }
    }

    bool m_isInteractable = false;
    Inventory m_inventory;
    MenuController m_menuController;
    GameManager m_gameManager;
    void Awake()
    {
        GameObject player = GameObject.Find("Player");

        m_inventory = player.GetComponent<Inventory>();
        m_craftingDeskWorkplace = m_workplace.GetComponent<CraftingDeskWorkplace>();

        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_menuController = m_gameManager.GetMenuController();
    }
    public override void Interact()
    {
        if (!m_isInteractable)
            return;

        m_menuController.InvokeCrafterDeskMenu();
        m_gameManager.m_currentCraftingDesk = GetComponent<CraftingDesk>();
    }
    public override string GetMessage(InputUnit interactionKey)
    {
        if (m_Blank != null)
        {
            m_isInteractable = false;
            return "";
        }

        if (m_inventory.m_selectedTool != null && m_inventory.m_selectedTool.m_item == m_logDescription)
        {
            m_isInteractable = true;
            return base.GetMessage(interactionKey) + "\ncreate blank";
        }
        m_isInteractable = false;
        return "";
    }
}
