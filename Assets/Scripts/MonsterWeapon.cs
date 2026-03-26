using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    [SerializeField] private Machete _machete;
    private Collider _collider;

    private void Awake()
    {
        _collider = _machete.GetComponent<BoxCollider>();
    }
    public void StartAttack()
    {
        _collider.enabled = true;
    }
    public void StopAttack()
    {
        _collider.enabled = false;
    }
}
