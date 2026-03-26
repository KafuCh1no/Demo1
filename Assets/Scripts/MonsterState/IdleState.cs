using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private MonsterFSM _manager;
    private Parameter _parameter;
    private float idleTime = 0;
    

    public IdleState(MonsterFSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }

    void IState.OnEnter()
    {
        _parameter._animator.CrossFade("Idle", 0.25f);
    }

    void IState.OnUpdate()
    {
        idleTime += Time.deltaTime;
        //Debug.Log($"idleTimeÊÇ£º{idleTime}");


        if(idleTime >= 3f)
        {
            _manager.Transition(EState.Patrol);

        }

        if (_manager.FoundPlayer())
        {
            _manager.Transition(EState.Chase);
        }
        
    }

    void IState.OnExit()
    {
        idleTime = 0f;
    }


}
