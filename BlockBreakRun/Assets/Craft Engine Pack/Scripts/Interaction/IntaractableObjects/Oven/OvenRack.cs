using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenRack : InteractableObject, IBlockable 
{
    Blank blankDescription;
    public Transform m_blankTransform;

    public event System.Action<Blank> OnBlankStateChanged;

    bool m_interactable = true;
    bool m_blocked = false;

    CraftCharacterController m_playerController;
    Inventory m_inventory;
    public Blank m_BlankDescription
    {
        get
        {
            return blankDescription;
        }
        set
        {
            blankDescription = null;
            if (value != null)
            {
                m_preLayout = Instantiate(value.m_prefab, m_blankTransform.position, m_blankTransform.rotation);
                DisableGameObjectDisturbComponents(m_preLayout);
                blankDescription = m_preLayout.GetComponent<InteractableObject>().m_itemDescription as Blank;
            }

            if (OnBlankStateChanged != null)
                OnBlankStateChanged(blankDescription);
        }
    }

    GameObject m_preLayout;
    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        m_playerController = player.GetComponent<CraftCharacterController>();
        m_inventory = player.GetComponent<Inventory>();
    }
    public override string GetMessage(InputUnit m_interactionKey)
    {
        if (m_blocked)
            return "";
        if(m_preLayout)
        {
            m_interactable = true;
            return base.GetMessage(m_interactionKey) + "\npick up blank";
        }

        if(m_playerController.m_Tool && m_playerController.m_Tool.GetComponent<InteractableObject>().m_itemDescription.GetType() == typeof(Blank))
        {
            m_interactable = true;
            return base.GetMessage(m_interactionKey) + "\nput blank";
        }

        m_interactable = false;
        return "";
    }
    public override void Interact()
    {
        if (!m_interactable || m_blocked)
            return;


        if(!m_preLayout)
        {
            m_BlankDescription = m_playerController.m_Tool.GetComponent<InteractableObject>().m_itemDescription as Blank;

            m_inventory.TakeAwaySelectedTool(1);
        }
        else
        {
            if (m_inventory.Put(m_BlankDescription))
            {
                Destroy(m_preLayout);
                m_BlankDescription = null;
            }
        }


        if (OnBlankStateChanged != null)
            OnBlankStateChanged(m_BlankDescription);
    }
    public void Block()
    {
        m_blocked = true;
    }
    public void UnBlock()
    {
        m_blocked = false;
    }
    public void DestroyBlank()
    {
        Destroy(m_preLayout.gameObject);
        if (OnBlankStateChanged != null)
            OnBlankStateChanged(null);
        m_BlankDescription = null;
    }
}
