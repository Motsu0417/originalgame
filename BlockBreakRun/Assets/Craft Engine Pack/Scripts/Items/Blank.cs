using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blank : ItemDescription {
    public float m_cuttingTime = 1.0f;
    public string m_blankDescription = "description of the blank";

    public float m_necessaryOreValue = 1f;
    public Layout m_futureObject;

    void Awake()
    {
        print(gameObject.name);
        print(gameObject.GetComponent<InteractableObject>() == null);
    }
    public override string ToString()
    {
        string res = "Name: " + m_name + "\n";
        res += "Description: " + m_blankDescription + "\n";
        res += "Future object: " + m_futureObject.m_name + "\n";
        res += "Time to cut: " + m_cuttingTime + "\n";
        res += "Necessary ore value to be smelt: " + m_necessaryOreValue + "\n";
        res += "Forge time: " + m_futureObject.m_forgeTime + "\n";
        res += "Ore type: " + m_futureObject.m_oreType.ToString();

        return res;
    }
    public bool Equals(Blank blank)
    {
        if (blank == null)
            return false;
        return base.Equals(blank) &&
            m_cuttingTime == blank.m_cuttingTime &&
            m_blankDescription == blank.m_blankDescription &&
            m_necessaryOreValue == blank.m_necessaryOreValue &&
            m_futureObject.m_forgeTime == blank.m_futureObject.m_forgeTime &&
            m_futureObject.Equals(blank.m_futureObject) &&
            m_futureObject.m_oreType == blank.m_futureObject.m_oreType;
    }
}
