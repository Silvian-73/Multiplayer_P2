﻿using UnityEngine;

public class AuraStrikeSkill : Skill
{
    [SerializeField] private int _damage;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _auraEffect;

    private Collider[] _bufferColliders = new Collider[64];

    protected override void OnUse()
    {
        if (isServer)
        {
            _unit.Motor.StopFollowingTarget();
        }
        base.OnUse();
    }
    protected override void OnCastComplete()
    {
        if (isServer)
        {
            Physics.OverlapSphereNonAlloc(transform.position, _radius, _bufferColliders, _enemyMask);
            for (int i = 0; i < _bufferColliders.Length; i++)
            {
                if (_bufferColliders[i] != null)
                {
                    Unit enemy = _bufferColliders[i].GetComponent<Unit>();
                    if (enemy != null && enemy.HasInteract)
                    {
                        enemy.TakeDamage(_unit.gameObject, _damage);
                    }
                }
            }
        }
        else
        {
            _auraEffect.Play();
        }
        base.OnCastComplete();
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}