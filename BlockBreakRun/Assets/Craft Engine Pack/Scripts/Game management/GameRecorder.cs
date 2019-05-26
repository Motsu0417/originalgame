using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class CellState
{
    public int m_count;
    public string m_itemDescriptionPrefabName;
}

[Serializable]
public class InventoryState
{
    public List<CellState> m_cells;
    public int m_selectedTool;
    public InventoryState()
    {
        m_cells = new List<CellState>();
    }
}

[Serializable]
class GameState
{
    public int m_sceneNumber { get; set; } //scene where player has save the game last time
    public Dictionary<int, SceneState> m_sceneStates { get; set; } //state of each scene
    public GameState()
    {
        m_sceneStates = new Dictionary<int, SceneState>();
    }
}

[Serializable]
class SceneState
{
    public SerVector3 m_playerPosition;
    public SerVector3 m_playerRotation;

    public List<State> m_sceneObjects; // all scene objects
    public List<State> m_dynamicObjects; // ll dynamic objects (created at runtime by player)

    public float m_notProcessedTime;

    public SceneState()
    {
        m_playerPosition = new SerVector3();
        m_playerRotation = new SerVector3();

        m_sceneObjects = new List<State>();
        m_dynamicObjects = new List<State>();
        m_notProcessedTime = 0.0f;
    }
}

public class GameRecorder : MonoBehaviour 
{
    public SaveAndLoadSettings m_saveAndLoadSettings; // save and load settings container
    List<Savable> m_dynamicObjects;
    public GameObject[] m_sceneObjectsContainers; // objects where gameSaver should search for savable scene objects
    List<Savable> m_sceneObjects;
    GameState m_gameState;
    public GameObject m_player; // player
    Inventory m_playerInventory;
    string m_dataPath; //data path to current world
    float m_startTime;
    int m_SceneIndex //current scene index
    {
        get
        {
            return Application.loadedLevel;
        }
    }
    SceneState m_SceneState
    {
        get
        {
            //return null or scene state from game state
            SceneState state;
            m_gameState.m_sceneStates.TryGetValue(m_SceneIndex, out state);
            return state;
        }
        set
        {
            //set new state or create it
            if(new List<int>(m_gameState.m_sceneStates.Keys).Contains(m_SceneIndex))
            {
                m_gameState.m_sceneStates[m_SceneIndex] = value;
            }
            else
            {
                m_gameState.m_sceneStates.Add(m_SceneIndex, value);
            }
        }
    }
    string m_SaveFolder//path to save folder
    {
        get { return m_dataPath + "/" + m_saveAndLoadSettings.m_savesFolder; }
    }
    string m_SaveFilePath
    {
        get 
        {
            //return temp save path if exists. If not -> return original save path
            if (Directory.Exists(m_SaveTempFolder))
            {
                return m_SaveTempFolder + "/" + m_saveAndLoadSettings.m_saveFileName;
            }
            return m_SaveFolder + "/" + m_saveAndLoadSettings.m_saveFileName; 
        }
    }
    string m_SaveTempFolder//path to temp folder
    {
        get { return m_dataPath + "/" + m_saveAndLoadSettings.m_temporarySavesFolder; }
    }
    string m_PlayerDataFilePath
    {

        get 
        {
            // return player data from temp folder if exists. If not -> return original
            if (Directory.Exists(m_SaveTempFolder))
                return m_SaveTempFolder + "/" + m_saveAndLoadSettings.m_playerSettingsFileName;
            return m_SaveFolder + "/" + m_saveAndLoadSettings.m_playerSettingsFileName; 
        }
    }
    void Awake()
    {
        m_dynamicObjects = new List<Savable>();
        m_dataPath = PlayerPrefs.GetString(m_saveAndLoadSettings.m_currentWorldVariableName);
        m_startTime = Time.time;
    }
    void Start()
    {
        m_playerInventory = m_player.GetComponent<Inventory>();

        F_LoadGameStateFromFile();
        F_LoadPlayerData();

        F_LoadSceneObjectsListFromScene();
        F_SceneObjectsProcessing();

        F_InstantiateDynamicObjects();
    }
    void F_PlayerDataProcessing()
    {
        if (m_SceneState == null)
            return;
        //set player position and rotation from file
        m_player.transform.position = m_SceneState.m_playerPosition.ToVector3();
        m_player.transform.rotation = Quaternion.Euler(m_SceneState.m_playerRotation.ToVector3());
    }
    void F_LoadGameStateFromFile()
    {
        m_gameState = new GameState();

        if (new DirectoryInfo(m_dataPath).GetDirectories().Length == 0)
        {
            Directory.CreateDirectory(m_SaveFolder);
            return;
        }


        FileStream fs;
        if (File.Exists(m_SaveFilePath))
        {
            fs = new FileStream(m_SaveFilePath, FileMode.Open);
        }
        else // if save file does not exist
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        //get game state from file
        m_gameState = (GameState)bf.Deserialize(fs);
        fs.Close();

        F_PlayerDataProcessing();
    }
    void F_LoadSceneObjectsListFromScene()
    {
        m_sceneObjects = new List<Savable>();

        //get objects from scene
        foreach (GameObject container in m_sceneObjectsContainers)
            if (container)
                foreach (Savable savable in container.GetComponentsInChildren(typeof(Savable), true))
                    if (savable.m_isSceneObject)
                        m_sceneObjects.Add(savable);
    }
    void F_SceneObjectsProcessing()
    {
        if(m_SceneState != null && m_SceneState.m_sceneObjects.Count != 0)
            for (int i = 0; i < m_sceneObjects.Count; ++i)
            {
                //set state from file for each scene object
                m_sceneObjects[i].HandleState(m_SceneState.m_sceneObjects[i]);
                if (m_sceneObjects[i].m_isTimeSensitive)
                    m_sceneObjects[i].ShiftTime(m_SceneState.m_notProcessedTime);
            }
    }
    public void F_SaveGameInOriginal()
    {
        //save game
        m_gameState.m_sceneNumber = m_SceneIndex;
        DeleteTempFolderIfExists();
        F_SaveGame();
    }

    public void F_SaveGameInTemp()
    {
        CreateTempFolder();
        F_SaveGame();
    }
    void F_SaveGame()
    {
        F_SaveSceneState();
        F_SavePlayerData();
    }
    void F_SaveSceneState()
    {
        m_SceneState = new SceneState();
        m_SceneState.m_sceneObjects = new List<State>();

        foreach (Savable s in m_sceneObjects)
        {
            s.OnSave();
            m_SceneState.m_sceneObjects.Add(s.m_state);//record scene objects' states
        }

        foreach (Savable s in m_dynamicObjects) // record dynamic objects' states
            if (s && !s.m_isFromFileOrInventory)
            {
                s.OnSave();
                m_SceneState.m_dynamicObjects.Add(s.m_state);
            }

        //record player position and rotation
        m_SceneState.m_playerPosition = new SerVector3(m_player.transform.position);
        m_SceneState.m_playerRotation = new SerVector3(m_player.transform.rotation.eulerAngles);

        foreach (int key in m_gameState.m_sceneStates.Keys)
        {
            if (m_gameState.m_sceneStates[key] != m_SceneState)
                m_gameState.m_sceneStates[key].m_notProcessedTime += Time.time - m_startTime;
        }
        m_SceneState.m_notProcessedTime = 0.0f;

        FileStream fs = new FileStream(m_SaveFilePath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        //write game state to file
        bf.Serialize(fs, m_gameState);
        fs.Close();
    }
    public void F_RegisterDynamicObject(Savable savObj)
    {
        m_dynamicObjects.Add(savObj);
    }
    void F_InstantiateDynamicObjects()
    {
        if (m_SceneState == null || m_SceneState.m_dynamicObjects.Count == 0)
            return;
        //create dynamic objects from file, then
        //set state for each of them
        foreach (State s in m_SceneState.m_dynamicObjects)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Items/" + s.m_itemDescriptionString).GetComponent<ItemDescription>().m_prefab);
            Savable savable = go.GetComponent<Savable>();
            savable.HandleState(s);
            if (savable.m_isTimeSensitive)
                savable.ShiftTime(m_SceneState.m_notProcessedTime);
        }
    }
    void F_LoadPlayerData()
    {
        if (!File.Exists(m_PlayerDataFilePath))
            return;
        //get player data from file
        FileStream fs = new FileStream(m_PlayerDataFilePath, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        m_playerInventory.InventoryState = (InventoryState)bf.Deserialize(fs);
        fs.Close();
    }
    void F_SavePlayerData()
    {
        //record player data to file
        FileStream fs = new FileStream(m_PlayerDataFilePath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, m_playerInventory.InventoryState);
        fs.Close();
    }
    public void DeleteTempFolderIfExists() //calls on exit and while save game
    {
        //deleting temp saves
        DirectoryInfo di = new DirectoryInfo(m_SaveTempFolder);
        if (!di.Exists)
            return;
        foreach (FileInfo f in di.GetFiles())
        {
            f.Delete();
        }
        di.Delete();
    }
    void CreateTempFolder()
    {
        if(!Directory.Exists(m_SaveTempFolder))
            Directory.CreateDirectory(m_SaveTempFolder);
    }
    void OnApplicationQuit()
    {
        DeleteTempFolderIfExists();
    }
}
