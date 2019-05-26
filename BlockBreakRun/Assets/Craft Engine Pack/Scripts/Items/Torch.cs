using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {
    public GameObject m_firePrefab;
    public Transform m_fireTarget;
    GameObject m_flame;
    void Start()
    {
        m_flame = Instantiate(m_firePrefab, m_fireTarget.position, m_firePrefab.transform.rotation);
        m_flame.GetComponent<FireMover>().m_Target = m_fireTarget;
    }
    void OnDestroy()
    {
        Destroy(m_flame);
    }
}
