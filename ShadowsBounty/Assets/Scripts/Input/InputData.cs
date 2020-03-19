using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class InputData : ScriptableObject
{
    public InputAction MovementAction;
    public InputAction LookAction;

    //Used to re-enable player controls after unpausing game, exiting a menu, etc.
    public void EnableControls()
    {
        MovementAction.Enable();
        LookAction.Enable();
    }

    //Use to disable player controls when pausing game, entering a menu, etc.
    public void DisableControls()
    {
        MovementAction.Disable();
        LookAction.Disable();
    }

    //Automatically called
    private void OnEnable()
    {
        EnableControls();
    }

    //Automatically called
    private void OnDisable()
    {
        DisableControls();
    }
}
