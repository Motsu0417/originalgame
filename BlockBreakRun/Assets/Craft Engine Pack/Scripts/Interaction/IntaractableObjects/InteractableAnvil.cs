using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAnvil : InteractableObject 
{
    public Transform m_layoutTransform;
    public HitableAnvil m_hitableAnvil;
    Inventory m_inventory;
    CraftCharacterController m_controller;
    bool m_interactable;
    bool m_blocked = false;
    GameObject m_LayoutObject
    {
        get
        {
            return m_hitableAnvil.m_LayoutObject;
        }
        set
        {
            m_hitableAnvil.m_LayoutObject = value;
        }
    }
    public Layout m_LayoutDescription
    {
        get
        {
            return m_hitableAnvil.m_LayoutDescription;
        }
        set
        {
            m_hitableAnvil.m_LayoutDescription = value;

            //instantiate, set position and rotation
            m_LayoutObject = Instantiate(value.m_prefab);

            float min = m_LayoutObject.transform.position.y;
            foreach (Collider c in m_LayoutObject.GetComponentsInChildren(typeof(Collider), true))
                min = Mathf.Min(min, c.bounds.min.y);
            float height = m_LayoutObject.transform.position.y - min;

            m_LayoutObject.transform.position = m_layoutTransform.position + new Vector3(0.0f, height, 0.0f);
            m_LayoutObject.transform.rotation = m_layoutTransform.rotation;

            DisableGameObjectDisturbComponents(m_LayoutObject);
        }
    }
    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        m_inventory = player.GetComponent<Inventory>();
        m_controller = player.GetComponent<CraftCharacterController>();
    }
    public override string GetMessage(InputUnit m_interactionKey)
    {
        if (m_blocked)
            return "";
        
        if(m_controller.m_Tool && m_controller.m_Tool.GetComponent<InteractableObject>().m_itemDescription.GetType() == typeof(Layout))
        {
            m_interactable = true;
            return base.GetMessage(m_interactionKey) + "\nput layout";
        }

        m_interactable = false;
        return "";
    }
    public override void Interact()
    {
        if (!m_interactable)
            return;

        if (!m_LayoutObject)
        {
            GameObject tool = m_controller.m_Tool;
            m_LayoutDescription = tool.GetComponent<InteractableObject>().m_itemDescription as Layout;

            m_inventory.TakeAwaySelectedTool(1);
        }
    }
}
