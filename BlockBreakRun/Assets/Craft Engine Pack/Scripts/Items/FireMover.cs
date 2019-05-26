using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMover : MonoBehaviour {
    public Transform m_Target { get; set; }
    public float m_speed = 10.0f;
    void Update()
    {
        if (m_Target)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Target.position, m_speed * Time.deltaTime);
        }
    }
}
