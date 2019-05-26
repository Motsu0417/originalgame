using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class InputUnit
{
    public delegate bool CheckerByKey(KeyCode key);
    public delegate bool SimpleChecker();

    bool enabled = true;
    public bool Enabled
    {
        get
        {
            return enabled;
        }
        set
        {
            enabled = value;
        }
    }
    public SimpleChecker InputSimpleChecker
    {
        get;
        set;
    }
    public CheckerByKey InputCheckerByKey //way to check input GetKeyDown | GetKey
    {
        get;
        set;
    }
    public KeyCode Key
    {
        get;
        set;
    }
    public event Action Pushed;

    public void ExecuteIfInput()
    {
        if (Enabled && Pushed != null)
        {
            bool res = true;
            if(InputCheckerByKey != null)
                res = res && InputCheckerByKey(Key);
            if (InputSimpleChecker != null)
                res = res && InputSimpleChecker();
            if(res)
                Pushed();
        }
    }
}

public class InputGroup
{
    public Dictionary<string, InputUnit> m_inputUnits { get; private set; }
    public bool Enabled { get; set; }
    public InputGroup()
    {
        Enabled = true;
        m_inputUnits = new Dictionary<string, InputUnit>();
    }
    public void Execute()
    {
        if (Enabled)
            foreach (InputUnit unit in m_inputUnits.Values)
                unit.ExecuteIfInput();
    }
}

public class InputManager : MonoBehaviour {
    //groups and concrete actions
    public const string MovingGroup = "Moving";
    public const string MoveForward = "Move forward";
    public const string MoveBack = "Move back";
    public const string MoveLeft = "Move left";
    public const string MoveRight = "Move right";
    public const string Jump = "Jump";

    public const string MenuGroup = "Menu";
    public const string Inventory = "Inventory";
    public const string Escape = "Escape";

    public const string InteractGroup = "Interact";
    public const string Interaction = "Interaction";
    public const string InteractionHit = "Hit";

    public const string ToolsActionsGroup = "Tool actions";
    public const string SelectToolWithScroll = "Select tool by scroll";
    public const string DropItem = "Drop item";

    public const string PlacementGroup = "Placement";
    public const string PlaceObject = "Place";
    public const string RotateObject = "Rotate";

    public Dictionary<string, InputGroup> m_inputGroups { get; private set; }

    //Controllers
    IMoving m_movingController;
    AccessViaRayCast m_interactor;
    public CraftCharacterController m_controller;
    public ToolActionController m_toolActionController;

    public MenuController m_menuController;

    void Awake()
    {
        m_inputGroups = new Dictionary<string, InputGroup>();

        m_movingController = GetComponent<IMoving>();//bind moving controller
        if (m_movingController == null)
            Debug.LogError("Person needs IMoving Component");
        else
            InitializeMovingInputs();

        m_interactor = GetComponent<AccessViaRayCast>();

        InitializeMenuInputs();
        InitializeInteractInputs();
        InitializeToolsActionsInputs();
        InitializePlacementInputs();

        m_menuController.GoToGame();
    }
    void InitializeMenuInputs()
    {
        InputGroup group = new InputGroup();

        //push escape -> call EscapeHandler
        InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Escape };
        unit.Pushed += m_menuController.EscapeHandler;
        group.m_inputUnits.Add(InputManager.Escape, unit);

        //push Tab -> call InventoryHandler
        unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Tab };
        unit.Pushed += m_menuController.InventoryHandler;
        group.m_inputUnits.Add(InputManager.Inventory, unit);

        m_inputGroups.Add(InputManager.MenuGroup, group);
    }

    void InitializeToolsActionsInputs()
    {
        InputGroup group = new InputGroup();
        KeyCode keyCode = KeyCode.Alpha1;
        InputUnit unit;

        //push '1' -> select first item on tool panel
        //push '6' -> select 6th item
        for(int i = 1; i <= 8; ++i, ++keyCode)
        {
            unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = keyCode };
            int j = i;
            unit.Pushed += () => m_controller.SetTool(j);
            group.m_inputUnits.Add(i.ToString(), unit);
        }

        //scroll down -> select next item on tool panel
        //scroll up -> select previous item on tool panel
        unit = new InputUnit();
        unit.Pushed += () =>
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0)
                m_controller.SelectPreviousTool();
            else if (scroll < 0)
                m_controller.SelectNextTool();
        };
        group.m_inputUnits.Add(InputManager.SelectToolWithScroll, unit);

        //push 'Q' -> drop item in hands
        unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Q };
        unit.Pushed += m_controller.DropSelectedItem;
        group.m_inputUnits.Add(InputManager.DropItem, unit);

        m_inputGroups.Add(InputManager.ToolsActionsGroup, group);
    }

    void InitializeInteractInputs()
    {
        InputGroup group = new InputGroup();

        //push 'E' -> item pick up executes
        InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.E };
        unit.Pushed += m_interactor.Interact;
        group.m_inputUnits.Add(InputManager.Interaction, unit);

        //click -> call item interaction
        unit = new InputUnit { InputCheckerByKey = (k) => !m_menuController.m_IsInMenu && Input.GetKey(k), Key = KeyCode.Mouse0 };
        unit.Pushed += new Action(() => { if (m_toolActionController) m_toolActionController.Hit(); });
        group.m_inputUnits.Add(InputManager.InteractionHit, unit);

        m_inputGroups.Add(InputManager.InteractGroup, group);
    }

    void InitializePlacementInputs()
    {
        InputGroup group = new InputGroup();

        //push 'F' -> call placement method
        InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.F };
        unit.Pushed += m_controller.ObjectPlacement;
        group.m_inputUnits.Add(InputManager.PlaceObject, unit);

        //push 'R' -> call rotating method in placement mode
        unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.R };
        unit.Pushed += m_controller.ObjectRotating;
        group.m_inputUnits.Add(InputManager.RotateObject, unit);

        m_inputGroups.Add(InputManager.PlacementGroup, group);
    }


    void InitializeMovingInputs()
    {
        InputGroup group = new InputGroup();

        InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.W };
        unit.Pushed += m_movingController.MoveForward;
        group.m_inputUnits.Add(InputManager.MoveForward, unit);

        unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.S };
        unit.Pushed += m_movingController.MoveBack;
        group.m_inputUnits.Add(InputManager.MoveBack, unit);

        unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.A };
        unit.Pushed += m_movingController.MoveLeft;
        group.m_inputUnits.Add(InputManager.MoveLeft, unit);

        unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.D };
        unit.Pushed += m_movingController.MoveRight;
        group.m_inputUnits.Add(InputManager.MoveRight, unit);

        unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Space };
        unit.Pushed += m_movingController.Jump;
        group.m_inputUnits.Add(InputManager.Jump, unit);

        m_inputGroups.Add(InputManager.MovingGroup, group);
    }

    // Update is called once per frame
    void Update()
    {
        m_movingController.ClearValue();
        foreach (InputGroup inputUnit in m_inputGroups.Values)
            inputUnit.Execute();
        m_movingController.Walk();
    }
    public InputUnit GetInputUnit(string group, string key)
    {
        return m_inputGroups[group].m_inputUnits[key];
    }
    public void SetActiveUnitGroup(string unitGroupName, bool activeOrNot)
    {
        m_inputGroups[unitGroupName].Enabled = activeOrNot;
    }
}
