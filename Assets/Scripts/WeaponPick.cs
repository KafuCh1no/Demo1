using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class WeaponPick : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform playerHand;
    public int itemId = 1;
    void IInteractable.OnHoverEnter()
    {
        
    }

    void IInteractable.OnHoverExit()
    {
        
    }

    void IInteractable.OnInteract(GameObject interactor)
    {
        PlayerLocomotionInput _playerLocomotionInput = interactor.GetComponent<PlayerLocomotionInput>();
        Animator _animator = interactor.GetComponent<Animator>();

        //transform.SetParent(playerHand);

        PackageLocalData.Instance.items.Add(new PackageLocalItem
        {
            uid = System.Guid.NewGuid().ToString(),
            id = itemId,
            num = 1,
            isNew = true
        });

        PackageLocalData.Instance.Save();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(5f, -90f, -75f);


        _playerLocomotionInput.AttackPressed = false;
        //_animator.SetBool("isArmed", true);
    }
}
