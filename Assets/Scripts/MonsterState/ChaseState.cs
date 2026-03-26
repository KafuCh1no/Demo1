using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private MonsterFSM _manager;
    private Parameter _parameter;

    public ChaseState(MonsterFSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }


    void IState.OnEnter()
    {
        _parameter._animator.CrossFade("Run", 0.25f);
    }

    void IState.OnUpdate()
    {
        Transform targetPoint = _parameter.playerTransform;
        float distance = Vector3.Distance(_manager.transform.position, _parameter.playerTransform.position);

        _manager.Move(targetPoint.position, "Run");

        if (!_manager.FoundPlayer())
        {
            
            _manager.Transition(EState.Patrol);
        }

        
        if(distance < _parameter.attackRadius)
        {
            _manager.Transition(EState.Attack);
        }

    }

    void IState.OnExit()
    {
        
    }
}
