using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ContainerElementAndDescription
{
    public ContainerElement m_element;
    public ItemDescription m_description;
    public ContainerElementAndDescriptionSerializible ToContElAndDescSerializible()
    {
        return new ContainerElementAndDescriptionSerializible
        {
            m_descriptionString = m_description.gameObject.name,
            m_elementData = this.m_element.m_data
        };
    }
}

public class OvenContainer : InteractableObject, IBlockable
{
    public event Action OnCountChanged;
    float targetCount;// target count to show it user
    bool m_hasTargetCount;
    public ContainerElement m_objectType;
    public List<ContainerElementAndDescription> m_containerElements { get; set; }

    CraftCharacterController m_playerController;
    Inventory m_inventory;

    Action m_handling;

    protected ContainerElement m_objectInHands;
    bool blocked = false;

    float m_GeneralValue
    {
        get
        {
            float val = 0.0f;
            m_containerElements.ForEach((ce) => val += ce.m_element.m_data.m_value);
            return val;
        }
    }

    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        m_TargetCount = -1f;
        m_playerController = player.GetComponent<CraftCharacterController>();
        m_inventory = player.GetComponent<Inventory>();
        m_containerElements = new List<ContainerElementAndDescription>();
    }

    public float m_TargetCount
    {
        get
        {
            return targetCount;
        }
        set
        {
            if (value < 0f)
                m_hasTargetCount = false;
            else
                m_hasTargetCount = true;
            targetCount = value;
        }
    }
    void PutObject()
    {
        m_containerElements.Add(new ContainerElementAndDescription
        {
            m_element = m_objectInHands,
            m_description = m_objectInHands.gameObject.GetComponent<InteractableObject>() ? m_objectInHands.gameObject.GetComponent<InteractableObject>().m_itemDescription : null
        });
        m_inventory.TakeAwaySelectedTool(1);
    }
    void PickUpObject()
    {
        ContainerElementAndDescription lastElement = m_containerElements[m_containerElements.Count - 1];
        m_inventory.Put(lastElement.m_description);
        m_containerElements.Remove(lastElement);
    }
    protected virtual bool AreCompatibleObjectInHandsAndContainerType()
    {
        return m_objectInHands && m_objectInHands.GetType() == m_objectType.GetType();
    }
    public override string GetMessage(InputUnit m_interactionKey)
    {
        if (blocked)
            return "";
        m_objectInHands = null;

        if(m_playerController.m_Tool)
            m_objectInHands = m_playerController.m_Tool.GetComponent<ContainerElement>();

        if (!AreCompatibleObjectInHandsAndContainerType())
            m_objectInHands = null;


        if (m_objectInHands != null)
            return SetPutInteraction(m_interactionKey);

        if (m_GeneralValue > 0)
            return SetPickUpInteraction(m_interactionKey);

        m_handling = null;
        return "";
    }

    string SetPickUpInteraction(InputUnit m_interactionKey) 
    {
        m_handling = PickUpObject;
        return base.GetMessage(m_interactionKey) + "\n" + m_GeneralValue + (m_hasTargetCount ? "/" + m_TargetCount : "") + "\npick up " + m_objectType.m_data.m_name;
    }
    string SetPutInteraction(InputUnit m_interactionKey) 
    {
        m_handling = PutObject;
        return base.GetMessage(m_interactionKey) + "\n" + m_GeneralValue + (m_hasTargetCount ? "/" + m_TargetCount : "") + "\nput " + m_objectType.m_data.m_name;
    }

    public void FillOvenWithElements(List<ContainerElementAndDescription> elems)
    {
        m_containerElements = elems;
        if (OnCountChanged != null)
            OnCountChanged();
    }

    public override void Interact()
    {
        if (!blocked && m_handling != null)
        {
            m_handling();
            if (OnCountChanged != null)
                OnCountChanged();
        }
    }
    public void DropAllElements()
    {

    }
    public void Block()
    {
        blocked = true;
    }
    public void UnBlock()
    {
        blocked = false;
    }
    public void RemoveTargetValue()
    {
        List<ContainerElementAndDescription> sortedList = new List<ContainerElementAndDescription>(m_containerElements);
        sortedList.Sort((el1, el2) => el2.m_element.m_data.m_value.CompareTo(el1.m_element.m_data.m_value));
        float value = 0.0f;
        for (int i = 0; value < m_TargetCount; ++i)
        {
            value += sortedList[i].m_element.m_data.m_value;
            m_containerElements.Remove(sortedList[i]);
        }
    }
}