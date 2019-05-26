using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class SceneSelector : MonoBehaviour 
{
    public SaveAndLoadSettings m_saveAndLoadSettings;
    public int m_defaultSceneIndex = 1; // default scene to load

    public GameObject m_buttonContainer;
    public GameObject m_buttonPrefab;

    public Button m_buttonCreate;
    public Button m_buttonSelect;
    public Button m_buttonDelete;

    public Text m_selectedWorldText;
    GameObject m_selectedButton;

    public InputField m_inputText;

    public PopUpMessage m_popUpMessage;

    List<string> m_existingWorlds;

    string m_WorldsFolder // folder where worlds data is stored
    {
        get { return Application.dataPath + "/" + m_saveAndLoadSettings.m_worldsFolder; }
    }
    string m_CurrentWorld // cur world folder
    {
        get { return m_WorldsFolder + "/" + m_selectedWorldText.text; }
    }
    void Start()
    {
        m_existingWorlds = new List<string>();
        m_selectedWorldText.text = "";
        DirectoryInfo worldsDir = new DirectoryInfo(m_WorldsFolder);
        //create folder for worlds if does not exist
        if (!worldsDir.Exists)
        {
            worldsDir.Create();
            return;
        }

        //get list of existing worlds;
        //create buttons for each world
        foreach (DirectoryInfo dir in worldsDir.GetDirectories())
        {
            GameObject butGo = Instantiate(m_buttonPrefab, m_buttonContainer.transform);
            Button but = butGo.GetComponent<Button>();
            but.GetComponentInChildren<Text>().text = dir.Name;
            m_existingWorlds.Add(dir.Name);
            but.onClick.AddListener(()=>OnWorldButtonClick(butGo));
        }
    }
    public void OnWorldButtonClick(GameObject button)
    {
        //set selected world
        m_selectedWorldText.text = button.GetComponentInChildren<Text>().text;
        SetInteractableSelectAndDeleteButtons(true);
        m_selectedButton = button;
    }
    public void OnCreateButtonClick() // button "Create" has been clicked
    {
        m_inputText.text = m_inputText.text.Trim();
        if (m_inputText.text == "")
        {
            m_popUpMessage.F_Show("Name of new world can't be empty");
            return;
        }
        if (m_existingWorlds.Contains(m_inputText.text))
        {
            m_popUpMessage.F_Show("Name of new world can't be the same as one of existing worlds");
            return;
        }

        Directory.CreateDirectory(m_WorldsFolder + "/" + m_inputText.text);
        LaunchLevel(m_WorldsFolder + "/" + m_inputText.text);
    }
    public void OnSelectButtonClick()
    {
        LaunchLevel(m_CurrentWorld);
    }
    void LaunchLevel(string path)
    {
        PlayerPrefs.SetString(m_saveAndLoadSettings.m_currentWorldVariableName, path);
        //if save file exists -> run saved scene
        //  if not -> run default scene
        if (!File.Exists(path + "/" + m_saveAndLoadSettings.m_savesFolder + "/" + m_saveAndLoadSettings.m_saveFileName))
            Application.LoadLevel(m_defaultSceneIndex);
        else
        {
            FileStream fs = new FileStream(path + "/" + m_saveAndLoadSettings.m_savesFolder + "/" + m_saveAndLoadSettings.m_saveFileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            GameState gs = (GameState)bf.Deserialize(fs);
            Application.LoadLevel(gs.m_sceneNumber);
            fs.Close();
        }
    }
    public void OnDeleteButtonClick() // if button "Delete" has been clicked
    {
        SetInteractableSelectAndDeleteButtons(false);
        Destroy(m_selectedButton);
        m_existingWorlds.Remove(m_selectedButton.GetComponentInChildren<Text>().text);
        DeleteDirectory(new DirectoryInfo(m_CurrentWorld));
    }
    void DeleteDirectory(DirectoryInfo dir)
    {
        foreach (DirectoryInfo d in dir.GetDirectories())
            DeleteDirectory(d);
        foreach (FileInfo file in dir.GetFiles())
            file.Delete();
        dir.Delete();
    }
    void SetInteractableSelectAndDeleteButtons(bool val)//enable or disable "delete" and "select" buttons
    {
        m_buttonDelete.interactable = val;
        m_buttonSelect.interactable = val;
    }
}
