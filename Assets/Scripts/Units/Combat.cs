using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[RequireComponent(typeof(UnitStats))]
public class Combat : NetworkBehaviour
{
    [SyncEvent] public event Action EventOnAttack;
    public float AttackDistance = 0f;
    [SerializeField] private float _attackSpeed = 1f;
    private float _attackCooldown = 0f;
    private UnitStats myStats;
    void Start()
    {
        myStats = GetComponent<UnitStats>();
    }
    private void Update()
    {
        if (_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
        }
    }
    public bool Attack(UnitStats targetStats)
    {
        if (_attackCooldown <= 0)
        {
            targetStats.TakeDamage(myStats.Damage.GetValue());
            EventOnAttack();
            _attackCooldown = 1f / _attackSpeed;
            return true;
        }
        return false;
    }
}
