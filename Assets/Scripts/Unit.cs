using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Unit : Interactable
{
    [SyncEvent] public event Action EventOnDamage;
    [SyncEvent] public event Action EventOnDie;
    [SyncEvent] public event Action EventOnRevive;

    [SerializeField] protected UnitMotor _motor;
    [SerializeField] protected UnitStats _stats;
    protected bool _isDead;
    protected Interactable _focus;

    public UnitStats Stats
    {
        get
        {
            return _stats;
        }
    }

    void Update()
    {
        OnUpdate();
    }
    protected virtual void OnAliveUpdate() { }
    protected virtual void OnDeadUpdate() { }
    protected void OnUpdate()
    {
        if (isServer)
        {
            if (!_isDead)
            {
                if (_stats.CurHealth == 0)
                {
                    Die();
                }
                else
                {
                    OnAliveUpdate();
                }
            }
            else
            {
                OnDeadUpdate();
            }
        }
    }

    [ClientRpc] void RpcDie()
    {
        if (!isServer) Die();
    }
    [ClientRpc] void RpcRevive()
    {
        if (!isServer) Revive();
    }

    [ClientCallback]
    protected virtual void Die()
    {
        _isDead = true;
        GetComponent<Collider>().enabled = false;
        if (isServer)
        {
            HasInteract = false;
            RemoveFocus();
            _motor.MoveToPoint(transform.position);
            EventOnDie();
            RpcDie();
        }
    }
    [ClientCallback]
    protected virtual void Revive()
    {
        _isDead = false;
        GetComponent<Collider>().enabled = false;
        if (isServer)
        {
            HasInteract = true;
            _stats.SetHealthRate(1);
            EventOnRevive();
            RpcRevive();
        }
    }
    protected virtual void SetFocus(Interactable newFocus)
    {
        if (newFocus != _focus)
        {
            _focus = newFocus;
            _motor.FollowTarget(newFocus);
        }
    }
    protected virtual void RemoveFocus()
    {
        _focus = null;
        _motor.StopFollowingTarget();
    }
    public override bool Interact(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null)
        {
            if (combat.Attack(_stats))
            {
                DamageWithCombat(user);
            }
            return true;
        }
        return base.Interact(user);
    }
    public override void OnStartServer()
    {
        _motor.SetMoveSpeed(_stats.MoveSpeed.GetValue());
        _stats.MoveSpeed.OnStatChanged += _motor.SetMoveSpeed;
    }
    protected virtual void DamageWithCombat(GameObject user)
    {
        EventOnDamage();
    }
}
