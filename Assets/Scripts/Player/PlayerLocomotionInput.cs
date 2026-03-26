using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, InputActions.IGameplayActions
{
    public InputActions PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    private bool _jumpPressed;
    public bool JumpPressed
    {
        get
        {
            bool temp = _jumpPressed;
            _jumpPressed = false;
            return temp;
        }
        set => _jumpPressed = value;
    }
    public bool SprintPressed { get; private set; }
    private bool _interactPressed = false;
    public bool InteractPressed
    {
        get
        {
            bool temp = _interactPressed;
            _interactPressed = false;
            return temp;
        }
        set => _interactPressed = value;
    }

    private bool _attackPressed = false;
    public bool AttackPressed
    {
        get
        {
            bool temp = _attackPressed;
            _attackPressed = false;
            return temp;
        }
        set => _attackPressed = value;
    }

    private bool _openPressed = false;
    public bool OpenPressed
    {
        get
        {
            bool temp = _openPressed;
            _openPressed = false;
            return temp;
        }
        set => _openPressed = value;
    }


    private void OnEnable()
    {
        PlayerControls = new InputActions();
        PlayerControls.Enable();

        PlayerControls.Gameplay.Enable();
        PlayerControls.Gameplay.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.Gameplay.Disable();
        PlayerControls.Gameplay.RemoveCallbacks(this);
    }

    private void LateUpdate()
    {
        JumpPressed = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpPressed = true;
        }

        
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SprintPressed = true;
        }
        else if (context.canceled)
        {
            SprintPressed = false;
        }
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractPressed = true;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackPressed = true;
        }
    }

    public void OnOpenPackage(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OpenPressed = true;
        }
    }
}
