using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStateLock : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var player = animator.GetComponent<PlayerController>();
        if (player != null)
        {
            player.canMove = false;
        } 
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var player = animator.GetComponent<PlayerController>();
        if (player != null)
        {
            player.canMove = true;
        }
    }
}
