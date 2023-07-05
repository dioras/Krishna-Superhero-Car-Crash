using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCG_InputManager {

    private static readonly BCG_Inputs inputs = new BCG_Inputs();
    private static BCG_InputActions inputActions;

    public delegate void onInteract();
    public static event onInteract OnInteract;

    public static BCG_Inputs GetInputs() {

        if (inputActions == null) {

            inputActions = new BCG_InputActions();
            inputActions.Enable();

            inputActions.Character.Interact.performed += Interact_performed;

        }

        if (!BCG_EnterExitSettings.Instance.mobileController) {

            inputs.horizonalInput = inputActions.Character.Movement.ReadValue<Vector2>().x;
            inputs.verticalInput = inputActions.Character.Movement.ReadValue<Vector2>().y;
            inputs.aim = inputActions.Character.Aim.ReadValue<Vector2>();

        }

        return inputs;

    }

    private static void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {

        if (OnInteract != null)
            OnInteract();

    }

}
