using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockable
{
    void Block();
    void UnBlock();
}

public class ContainerElement : MonoBehaviour 
{
    public ContainerData m_data;
}

[System.Serializable]
public class ContainerData
{
    public string m_name;
    public float m_value;
}
