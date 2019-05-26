using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour {
    public Text m_text;
    public float m_alphaDecrementStep = 0.5f;// this determines speed of text fading
    float m_Alpha //set alpha of pop up text
    {
        get { return m_text.color.a; }
        set
        {
            Color color = m_text.color;
            color.a = value;
            m_text.color = color;
        }
    }
    void Awake()
    {
        m_text.text = "";
    }
    public void F_Show(string text)
    {
        m_Alpha = 1;
        m_text.text = text;
    }
    void Update()
    {
        if(m_Alpha > 0.0f)
        {
            //fading
            m_Alpha -= Mathf.Min(m_Alpha, m_alphaDecrementStep * Time.deltaTime);
        }
    }
}
