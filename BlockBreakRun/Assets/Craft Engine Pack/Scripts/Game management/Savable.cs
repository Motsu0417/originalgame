using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SerVector3 //serializable Vector3
{
    public float x, y, z;
    public SerVector3() : this(new Vector3()) { }
    public SerVector3(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public class State //ususal savable object's state
{
    public bool m_alive = true;// if not alive -> it'll be destroyed while state handling
    public string m_itemDescriptionString { get; set; }//prefab of object to be created
    public SerVector3 m_position { get; set; }
    public SerVector3 m_rotation { get; set; }

    public State() { }
    public State(State state)
    {
        m_alive = state.m_alive;
        m_position = new SerVector3();
        m_rotation = new SerVector3();
    }
}
public class Savable : MonoBehaviour 
{
    [HideInInspector]
    public State m_state;// this state is for game saver
    public bool m_isSceneObject;
    public bool m_isTimeSensitive = false;
    public ItemDescription m_itemDescription;
    public bool m_isFromFileOrInventory { get; set; }
    void Awake()
    {
        InitializeState();
    }
    void Start()
    {
        if (!m_isSceneObject && !m_isFromFileOrInventory) // if dynamic object -> register it in game saver
            GameObject.Find("GameSaver").GetComponent<GameRecorder>().F_RegisterDynamicObject(GetComponent<Savable>());
    }
    void RecordRotationAndPosition()//refresh rotation and position
    {
        m_state.m_position = new SerVector3(transform.position);
        m_state.m_rotation = new SerVector3(transform.rotation.eulerAngles);
    }
    protected virtual void InitializeState()
    {
        m_state = new State();
    }
    public virtual void HandleState(State state)
    {
        //do smth with your object
        if (!state.m_alive)
            Destroy(gameObject);

        if (m_isSceneObject)
            return;

        transform.position = state.m_position.ToVector3();
        transform.rotation = Quaternion.Euler(state.m_rotation.ToVector3());
    }
    void OnDestroy()
    {
        m_state.m_alive = false;
    }
    public virtual void ShiftTime(float time)
    {

    }
    public virtual void OnSave() 
    {
        if(!m_isSceneObject)
        RecordRotationAndPosition();

        if (m_itemDescription)
            m_state.m_itemDescriptionString = m_itemDescription.gameObject.name;
    }
}
