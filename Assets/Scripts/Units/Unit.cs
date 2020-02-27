using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Unit : Interactable
{
    public event Action EventOnDamage;
    public event Action EventOnDie;
    public event Action EventOnRevive;

    [SerializeField] private UnitMotor _motor;
    [SerializeField] private UnitStats _stats;

    public UnitMotor Motor 
    { 
        get 
        { 
            return _motor; 
        } 
    }

    public UnitStats Stats 
    { 
        get 
        { 
            return _stats; 
        } 
    }
    public UnitSkills UnitSkills;

    protected float _interactDistance;
    protected bool _isDead;
    protected Interactable _focus;
    public Interactable Focus
    { 
        get 
        { 
            return _focus; 
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
    protected virtual void Die()
    {
        _isDead = true;
        GetComponent<Collider>().enabled = false;
        EventOnDie();
        if (isServer)
        {
            HasInteract = false;
            RemoveFocus();
            _motor.MoveToPoint(transform.position);
            RpcDie();
        }
    }
    protected virtual void Revive()
    {
        _isDead = false;
        GetComponent<Collider>().enabled = true;
        EventOnRevive();
        if (isServer)
        {
            HasInteract = true;
            _stats.SetHealthRate(1);
            RpcRevive();
        }
    }
    public virtual void SetFocus(Interactable newFocus)
    {
        if (newFocus != _focus)
        {
            _focus = newFocus;
            _interactDistance = _focus.GetInteractDistance(gameObject);
            _motor.FollowTarget(newFocus, _interactDistance);
        }
    }
    public virtual void RemoveFocus()
    {
        _focus = null;
        _motor.StopFollowingTarget();
    }

    public override float GetInteractDistance(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        return base.GetInteractDistance(user) + (combat != null ? combat.AttackDistance : 0f);
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
    public void TakeDamage(GameObject user, int damage)
    {
        _stats.TakeDamage(damage);
        DamageWithCombat(user);
    }
    public void UseSkill(int skillNum)
    {
        if (!_isDead && skillNum < UnitSkills.Count)
        {
            UnitSkills[skillNum].Use(this);
        }
    }
}
