using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenCentralController : MonoBehaviour {
    public OvenContainer m_fuelContainer;
    public OvenOreContainer m_oreContainer;
    public OvenRack m_rack;

    public GameObject m_fire;

    Blank m_preLayout;

    float m_fuelContainerValue;
    float m_oreContainerValue;
    float m_burningTime;

    float m_bakingStartTime;
    public bool m_IsBaking { get; private set; }
    public float m_BakingTime
    {
        get
        {
            return Time.time - m_bakingStartTime;
        }
    }
    public float m_FinishBakingTime { get; set; }

    public bool m_IsLoading { get; set; } // is loading from save file processing now?

    bool Burn
    {
        set
        {
            m_fire.SetActive(value);
        }
    }

    void FuelContainerFuelValueChangedHandler()
    {
        m_fuelContainerValue = 0.0f;
        m_burningTime = 0.0f;
        //count general fuel value and burning time
        m_fuelContainer.m_containerElements.ForEach((cead) => {
            Fuel fuel = (Fuel)cead.m_element;
            m_fuelContainerValue += fuel.m_data.m_value;
            m_burningTime += fuel.m_burningTime;
        });
        TryStartBaking();
    }
    void OreContainerValueChangedHandler()
    {
        float targetFuel = 0.0f;
        m_oreContainerValue = 0.0f;

        //calculate ore value and necessary fuel value
        m_oreContainer.m_containerElements.ForEach((el) =>
        {
            Ore ore = (Ore)el.m_element;
            if((m_oreContainerValue += ore.m_data.m_value) <= m_oreContainer.m_TargetCount)
                targetFuel += ore.m_necessaryFuelValue;
        });

        m_fuelContainer.m_TargetCount = targetFuel;

        TryStartBaking();
    }
    void RackPreLayoutChangedHandler(Blank preLayout)
    {
        m_preLayout = preLayout;

        //calculate necessary ore value
        if (preLayout)
        {
            if (m_oreContainer.m_OreType != preLayout.m_futureObject.m_oreType)
                m_oreContainer.DropAllElements();
            m_oreContainer.m_OreType = m_preLayout.m_futureObject.m_oreType;
            m_oreContainer.m_TargetCount = m_preLayout.m_necessaryOreValue;
            TryStartBaking();
        }
    }
    public void TryStartBaking()
    {
        if (!m_preLayout || m_IsLoading)
            return;
        if (m_fuelContainer.m_TargetCount < 0 || m_oreContainer.m_TargetCount < 0)
            return;

        
        if (m_fuelContainerValue >= m_fuelContainer.m_TargetCount && m_oreContainerValue >= m_preLayout.m_necessaryOreValue)
        {
            Bake(m_burningTime);
        }
    }

    public void Bake(float timeToBake)
    {
        m_bakingStartTime = Time.time;
        m_FinishBakingTime = Time.time + timeToBake;
        m_IsBaking = true;
        //block rack's and containers' interaction
        m_rack.Block();
        m_fuelContainer.Block();
        m_oreContainer.Block();
        Burn = true;
    }
    void Update()
    {
        if (!m_IsBaking)
            return;
        if (Time.time >= m_FinishBakingTime)
            FinishBaking();
    }

    void FinishBaking()
    {
        m_IsBaking = false;
        Burn = false;
        //instantiate layout
        Instantiate(m_preLayout.m_futureObject.m_prefab, m_rack.m_blankTransform.position, m_rack.m_blankTransform.rotation);
        m_rack.DestroyBlank();
        //remove resources from containers
        m_fuelContainer.RemoveTargetValue();
        m_oreContainer.RemoveTargetValue();

        FuelContainerFuelValueChangedHandler();
        OreContainerValueChangedHandler();

        m_rack.UnBlock();
        m_fuelContainer.UnBlock();
        m_oreContainer.UnBlock();
    }
    void Awake()
    {
        m_fuelContainer.OnCountChanged += FuelContainerFuelValueChangedHandler;
        m_oreContainer.OnCountChanged += OreContainerValueChangedHandler;
        m_rack.OnBlankStateChanged += RackPreLayoutChangedHandler;
        m_IsBaking = false;
    }
}
