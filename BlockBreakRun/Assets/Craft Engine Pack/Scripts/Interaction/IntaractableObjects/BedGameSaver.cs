using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedGameSaver : InteractableObject {
    public override void Interact()
    {
        GameObject.Find("GameSaver").GetComponent<GameRecorder>().F_SaveGameInOriginal();
    }
    public override string GetMessage(InputUnit m_interactionKey)
    {
        return base.GetMessage(m_interactionKey) + "\nsave game";
    }
}
