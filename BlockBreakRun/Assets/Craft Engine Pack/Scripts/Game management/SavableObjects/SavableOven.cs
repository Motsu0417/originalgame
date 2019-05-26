using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContainerElementAndDescriptionSerializible
{
    public ContainerData m_elementData;
    public string m_descriptionString;
    public ContainerElementAndDescription ToContElAndDesc()
    {
        ContainerElementAndDescription cead = new ContainerElementAndDescription
        {
            m_description = Resources.Load<GameObject>("Items/" + m_descriptionString).GetComponent<ItemDescription>()
        };
        cead.m_element = cead.m_description.m_prefab.GetComponent<ContainerElement>();
        return cead;
    }
}

[System.Serializable]
public class OvenState : State
{
    public List<ContainerElementAndDescriptionSerializible> m_OreElems;
    public List<ContainerElementAndDescriptionSerializible> m_FuelElems;
    public string m_blankDescriptionString;
    public float m_bakedTime;
    public OvenState()
    {
        m_OreElems = new List<ContainerElementAndDescriptionSerializible>();
        m_FuelElems = new List<ContainerElementAndDescriptionSerializible>();
        m_blankDescriptionString = "";
        m_bakedTime = 0.0f;
    }
}
public class SavableOven : Savable 
{
    OvenState m_ovenState;
    OvenCentralController m_oven;
    
    protected override void InitializeState()
    {
        m_ovenState = new OvenState();
        m_state = m_ovenState;

        m_oven = GetComponent<OvenCentralController>();
    }
    public override void HandleState(State state)
    {
        m_oven.m_IsLoading = true;
        base.HandleState(state);

        m_ovenState = (OvenState)state;
        m_state = m_ovenState;

        List<ContainerElementAndDescription> ores = new List<ContainerElementAndDescription>();
        List<ContainerElementAndDescription> fuels = new List<ContainerElementAndDescription>();

        if (m_ovenState.m_OreElems != null)
            m_ovenState.m_OreElems.ForEach((element) =>
            {
                ores.Add(element.ToContElAndDesc());
            });
        if (m_ovenState.m_FuelElems != null)
            m_ovenState.m_FuelElems.ForEach((element) =>
            {
                fuels.Add(element.ToContElAndDesc());
            });

        m_oven.m_fuelContainer.FillOvenWithElements(fuels);
        m_oven.m_oreContainer.FillOvenWithElements(ores);

        m_oven.m_rack.m_BlankDescription = m_ovenState.m_blankDescriptionString != "" ?
            Resources.Load<GameObject>("Items/" + m_ovenState.m_blankDescriptionString).GetComponent<Blank>() :
            null;
        m_oven.m_IsLoading = false;
        m_oven.TryStartBaking();
    }
    public override void ShiftTime(float time)
    {
        if(m_oven.m_IsBaking)
        {
            m_oven.m_FinishBakingTime -= time + m_ovenState.m_bakedTime;
        }
    }
    public override void OnSave()
    {
        base.OnSave();

        m_ovenState.m_OreElems = new List<ContainerElementAndDescriptionSerializible>();
        m_ovenState.m_FuelElems = new List<ContainerElementAndDescriptionSerializible>();

        m_oven.m_oreContainer.m_containerElements.ForEach((element) =>
        {
            m_ovenState.m_OreElems.Add(element.ToContElAndDescSerializible());
        });
        m_oven.m_fuelContainer.m_containerElements.ForEach((element) =>
        {
            m_ovenState.m_FuelElems.Add(element.ToContElAndDescSerializible());
        });

        m_ovenState.m_blankDescriptionString = m_oven.m_rack.m_BlankDescription != null ? m_oven.m_rack.m_BlankDescription.gameObject.name : "";
        m_ovenState.m_bakedTime = m_oven.m_BakingTime;
    }
}
