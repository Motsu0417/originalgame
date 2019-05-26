using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : InteractableObject {
    public Transform m_preSavePlayerTransform;
    public Transform m_playerTransform;
    public int m_sceneIndexToLoad = 1;
    public override void Interact()
    {
        if (!m_playerTransform)
            m_playerTransform = GameObject.Find("Player").transform;
        if (!m_preSavePlayerTransform)
            m_preSavePlayerTransform = m_playerTransform;

        //set player position and rotation for next entry in the scene
        m_playerTransform.position = m_preSavePlayerTransform.position;
        m_playerTransform.rotation = m_preSavePlayerTransform.rotation;
        //save game into TEMP folder (save state between scenes)
        GameObject.Find("GameSaver").GetComponent<GameRecorder>().F_SaveGameInTemp();
        Application.LoadLevel(m_sceneIndexToLoad);
    }
}
