using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour 
{
    public GameObject m_inventoryMenu; //container with inventory, equipment grids and crafter
    public CraftCharacterController m_controller;
    public bool m_IsInMenu
    {
        get
        {
            bool res = false;
            res = res || m_inventoryMenu.active;
            res = res || m_crafterDeskMenu.active;
            res = res || m_menu.active;

            return res;
        }
    }
    public InputManager m_inputManager;
    public HeadRotationWithMouseController m_rotationWithMouseController;

    public int m_menuSceneIndex = 0;// index of preloader's scene

    public GameObject m_menu;

    public GameObject m_crafterDeskMenu;
    public GameObject m_chestMenu;
    public GameObject m_crafterDeskMenuButtonContainer;
    public GameObject m_crafterDeskMenuButtonPrefab;
    public Button m_buttonBack;
    public Button m_buttonCreateBlank;
    public Text m_blankDescriptionText;
    public GameManager m_gameManager;
    public GameObject m_crafter;

    Blank m_selectedBlank;

    void Start()
    {
        FillCrafterDeskMenuButtonContainer();
    }
    public void EscapeHandler()
    {
        if (m_IsInMenu)
        {
            if (!m_controller.m_inventory.m_isItPossibleToGoToGame)
                return;
            SetActiveInventoryHandler(true);
            GoToGame();
        }
        else // go to menu
        {
            //code to pause game
            SetActiveInventoryHandler(false);
            GoToMenu();
            m_menu.SetActive(true);
        }
    }
    public void InventoryHandler()
    {
        if (m_inventoryMenu.active) // switch off inventory
        {
            if (!m_controller.m_inventory.m_isItPossibleToGoToGame)
                return;
            GoToGame();
        }
        else
        {
            m_inventoryMenu.SetActive(true); //switch on inventory
            m_chestMenu.SetActive(false);
            m_crafter.SetActive(true);
            GoToMenu();
        }
    }
    void GoToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_inputManager.SetActiveUnitGroup(InputManager.MovingGroup, false);
        m_inputManager.SetActiveUnitGroup(InputManager.InteractGroup, false);
        m_inputManager.SetActiveUnitGroup(InputManager.PlacementGroup, false);
        m_inputManager.SetActiveUnitGroup(InputManager.ToolsActionsGroup, false);
        m_rotationWithMouseController.Enabled = false;
    }
    public void GoToGame()
    {
        m_controller.DisablePlacementMode();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_inputManager.SetActiveUnitGroup(InputManager.MovingGroup, true);
        m_inputManager.SetActiveUnitGroup(InputManager.InteractGroup, true);
        m_inputManager.SetActiveUnitGroup(InputManager.PlacementGroup, true);
        m_inputManager.SetActiveUnitGroup(InputManager.ToolsActionsGroup, true);
        m_rotationWithMouseController.Enabled = true;
        DisableMenus();
    }
    public void MenuContinueButtonClicked()
    {
        EscapeHandler();
    }
    public void MenuEndGameButtonClicked()
    {
        GameObject.Find("GameSaver").GetComponent<GameRecorder>().DeleteTempFolderIfExists();
        Application.LoadLevel(m_menuSceneIndex);
    }
    void FillCrafterDeskMenuButtonContainer()
    {
        foreach (Blank blank in Resources.LoadAll<Blank>("Items"))
        {
            GameObject go = Instantiate(m_crafterDeskMenuButtonPrefab, m_crafterDeskMenuButtonContainer.transform);
            go.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture>("Sprites/" + blank.m_sprite);
            Text text = go.GetComponentInChildren(typeof(Text), true) as Text;
            text.text = blank.name;
            go.GetComponentInChildren<Button>().onClick.AddListener(() => CrafterDeskMenuButtonClicked(blank));
        }
    }
    void CrafterDeskMenuButtonClicked(Blank blank)
    {
        m_blankDescriptionText.text = blank.ToString();
        m_buttonCreateBlank.interactable = true;
        m_selectedBlank = blank;
    }
    public void InvokeCrafterDeskMenu()
    {
         SetActiveInventoryHandler(false);
        m_buttonCreateBlank.interactable = false;
        m_blankDescriptionText.text = "";
        m_crafterDeskMenu.SetActive(true);
        GoToMenu();
    }
    public void InvokeChestMenu()
    {
        m_inventoryMenu.SetActive(true);
        m_crafter.SetActive(false);
        m_chestMenu.SetActive(true);
        GoToMenu();
    }
    void DisableMenus()
    {
        m_menu.SetActive(false);
        m_crafterDeskMenu.SetActive(false);
        m_inventoryMenu.SetActive(false);
    }
    void SetActiveInventoryHandler(bool value)
    {
        m_inputManager.m_inputGroups[InputManager.MenuGroup].m_inputUnits[InputManager.Inventory].Enabled = value; ;
    }
    public void CrafterDeskMenuCreateBlankButtonClicked()
    {
        m_gameManager.F_SetBlank(m_selectedBlank);
        EscapeHandler();
    }
}
