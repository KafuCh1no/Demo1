using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour, IInteractable
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void IInteractable.OnHoverEnter()
    {
        
    }

    void IInteractable.OnHoverExit()
    {

    }

    void IInteractable.OnInteract(GameObject interactor)
    {
        _animator.SetTrigger("Pressed");
        bool currentState = _animator.GetBool("isOpen");
        bool newState = !currentState;

        _animator.SetBool("isOpen", newState);
    }
}
