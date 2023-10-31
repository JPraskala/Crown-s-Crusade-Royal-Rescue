using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Basic Components for Input")]
    Game inputActions;
    PlayerInput input;
    public static InputManager inputInstance;
    const string UI = "UI";
    const string Player = "Player";

    public static InputManager Instance
    {
        get
        {
            if (inputInstance == null)
            {
                inputInstance = FindObjectOfType<InputManager>();

                if (inputInstance == null)
                {
                    GameObject obj = new("InputManager");
                    inputInstance = obj.AddComponent<InputManager>();
                }
            }
            return inputInstance;
        }
    }

    void Awake()
    {
        if (inputInstance == null)
        {
            inputInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        if (!TryGetComponent<PlayerInput>(out input))
        {
            input = gameObject.AddComponent<PlayerInput>();
        }

        inputActions = new Game();

        inputActions.Enable();
    }

    void Update() 
    {
        float horizontalInput = inputActions.Player.Move.ReadValue<float>();
        float jumpInfo = inputActions.Player.Jump.ReadValue<float>();
    }

    public void SwapActionMap(char sceneCharacter)
    {
        string activeMap = GetActionMap();
        if (sceneCharacter == 'L') 
        {
            if (activeMap != Player) 
            {
                input.SwitchCurrentActionMap(Player);
            }
        }
        else 
        {
            if (activeMap != UI) 
            {
                input.SwitchCurrentActionMap(UI);
            }
        }

        print("Current action map is " + GetActionMap());
    }

    public string GetActionMap() 
    {
        return input.currentActionMap.name;
    }
}
