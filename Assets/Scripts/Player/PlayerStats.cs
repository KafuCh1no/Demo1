using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _animator.SetTrigger("isAttacked");
        currentHealth -= damage;
        Debug.Log($"玩家当前剩余血量{currentHealth}");
    }
}
