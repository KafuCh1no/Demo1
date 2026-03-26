using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private Animator _animator;
    private Collider _collider;
    private MonsterFSM _manager;

    private void Awake()
    {
        _manager = GetComponent<MonsterFSM>();
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        
    }

    public void GotHit(float damage)
    {
        MonsterHitEffect hitEffect = GetComponent<MonsterHitEffect>();

        _manager.moveSpeed = 0;
        _animator.CrossFade("GotHit", 0.25f);
        currentHealth -= damage;
        Debug.Log($"怪物当前剩余血量{currentHealth}");
        
        if (hitEffect != null)
        {
            hitEffect.PlayHitFlash();
        }
    }
}
