using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class AttackState : IState
{
    private MonsterFSM _manager;
    private Parameter _parameter;
    //这个attackAnimation为攻击的动画时间

    private float attackInterval;
    private float maxAttackInterval = 1.5f;

    public AttackState(MonsterFSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }

    void IState.OnEnter()
    {
        _parameter._animator.CrossFade("Attack", 0.25f);
        _parameter.lastAttack = Time.time;
    }

    void IState.OnUpdate()
    {
        attackInterval += Time.deltaTime;
        if(attackInterval >= maxAttackInterval)
        {
            _manager.Transition(EState.Chase);
        }
    }

    void IState.OnExit()
    {
        attackInterval = 0f;
    }
}
