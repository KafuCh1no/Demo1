using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : MonoBehaviour, IInteractable
{
    [SerializeField] private TextAsset _textAsset;


    void IInteractable.OnHoverEnter()
    {
        
    }

    void IInteractable.OnHoverExit()
    {
        
    }

    void IInteractable.OnInteract(GameObject interactor)
    { 
        DialogManager.Instance.UpdateDialog(_textAsset, this.transform);
    }

}
