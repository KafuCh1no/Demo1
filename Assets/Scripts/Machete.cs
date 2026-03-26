using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Machete : MonoBehaviour
{
    [SerializeField] private Transform playerHand;
    private float damage = 10f;
    private Collider _collider;
    public int itemId = 1;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            stats.TakeDamage(damage);
            _collider.enabled = false;
        }
        if (other.CompareTag("Monster"))
        {
            MonsterStats stats = other.GetComponent<MonsterStats>();
            stats.GotHit(damage);
            _collider.enabled = false;
        }
        
    }

}
