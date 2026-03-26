using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private MonsterFSM _manager;
    private Parameter _parameter;
    
    private int patrolIndex;

    public PatrolState(MonsterFSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        
    }

    void IState.OnEnter()
    {
        _parameter._animator.CrossFade("Walk", 0.25f);
    }

    void IState.OnUpdate()
    {
        Transform targetPoint = _parameter.patrolPoints[patrolIndex];

        _manager.Move(targetPoint.position, "Walk");

        if (Vector3.Distance(_manager.transform.position, targetPoint.position) < 0.5f)
        {
            //切换到下一个点
            patrolIndex = (patrolIndex + 1) % _parameter.patrolPoints.Length;

            //先回到 Idle 待机一会儿再走
            _manager.Transition(EState.Idle);
        }

        if (_manager.FoundPlayer())
        {
            _manager.Transition(EState.Chase);
        }


    }

    void IState.OnExit()
    {

    }

    
}
