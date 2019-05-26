using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadSettings : MonoBehaviour // just container for save and load settings
{
    public string m_worldsFolder = "Worlds";
    public string m_savesFolder = "Saves";
    public string m_temporarySavesFolder = "TempSaves";
    public string m_saveFileName = "main.save";
    public string m_playerSettingsFileName = "player.save";
    public string m_currentWorldVariableName = "currentWorld";
}
