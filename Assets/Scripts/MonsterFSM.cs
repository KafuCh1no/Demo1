using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing.Text;
using TMPro;
using UnityEngine;

[Serializable]
public class Parameter
{
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public float gravity;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
    public Animator _animator;

    public float viewRadius;
    public float viewAngle;
    public float attackRadius;
    public Transform playerTransform;

    public float attackInterval;
    public float lastAttack;
}

public enum EState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Die
}


public class MonsterFSM : MonoBehaviour
{
    public Parameter parameter;
    private IState currentState;
    private CharacterController _characterController;
    public float moveSpeed;

    private Dictionary<EState, IState> states = new Dictionary<EState, IState>();

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        states.Add(EState.Idle, new IdleState(this));
        states.Add(EState.Patrol, new PatrolState(this));
        states.Add(EState.Chase, new ChaseState(this));
        states.Add(EState.Attack, new AttackState(this));
    }

    private void Start()
    {
        Transition(EState.Idle);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void Transition(EState targetState)
    {
        if(currentState != null)
        {
            currentState.OnExit();
        }

        currentState = states[targetState];
        currentState.OnEnter();
    }

    public void Move(Vector3 targetPosition, string moveMent)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        if (moveMent == "Walk")
        {
            moveSpeed = parameter.walkSpeed;
        }else if(moveMent == "Run")
        {
            moveSpeed = parameter.runSpeed;
        }

        Vector3 moveDelta = direction * moveSpeed * Time.deltaTime;
        moveDelta.y = -parameter.gravity * Time.deltaTime;
        _characterController.Move(moveDelta);
        //Debug.Log($"正在移动！目标：{targetPosition}，速度为：{_parameter.walkSpeed},当前位置：{_manager.transform.position}");
    }

    public bool FoundPlayer()
    {
        float distance = Vector3.Distance(transform.position, parameter.playerTransform.position);
        if (distance > parameter.viewRadius)
        {
            return false;
        }
        
        

        Vector3 directionToPlayer = (parameter.playerTransform.position - transform.position).normalized;

        float dot = Vector3.Dot(transform.forward.normalized, directionToPlayer);

        if (dot >= Mathf.Cos(parameter.viewAngle * 0.5f * Mathf.Deg2Rad))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
