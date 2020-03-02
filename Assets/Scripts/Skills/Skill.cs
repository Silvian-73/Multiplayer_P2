using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Skill : NetworkBehaviour
{
    public Sprite icon;

    [SerializeField] protected float _castTime = 1f;
    [SerializeField] protected float _cooldown = 1f;
    [HideInInspector] public float CastDelay;
    [HideInInspector] public float CooldownDelay;
    protected Unit _unit;
    protected Interactable _target;

    private List<Skill> _skills;
    private int _skillIndex;

    protected virtual void Start()
    {
        _skills = new List<Skill>(GetComponents<Skill>());
        _skillIndex = _skills.FindIndex(x => x == this);
    }

    void Update()
    {
        if (CastDelay > 0)
        {
            CastDelay -= Time.deltaTime;
            if (CastDelay <= 0)
            {
                CastDelay = 0;
                if (isServer) OnCastComplete();
            }
        }
        if (CooldownDelay > 0)
        {
            CooldownDelay -= Time.deltaTime;
            if (CooldownDelay <= 0)
            {
                CooldownDelay = 0;
                if (isServer) OnCooldownComplete();
            }
        }
    }

    public void Use(Unit unit)
    {
        if (CastDelay == 0 && CooldownDelay == 0)
        {
            _unit = unit;
            _target = unit.Focus;
            OnUse();
        }
    }

    protected virtual void OnUse()
    {
        if (isServer)
        {
            RpcOnUse(_skillIndex, _unit.gameObject, _target != null ? _target.gameObject : null);
            if (_castTime > 0)
            {
                CastDelay = _castTime;
            }
            else
            {
                OnCastComplete();
            }
        }
        else
        {
            if (_castTime > 0)
            {
                CastDelay = _castTime;
            }
        }
    }

    protected virtual void OnCastComplete()
    {
        if (isServer)
        {
            RpcOnCastComplete(_skillIndex);
            if (_cooldown > 0)
            {
                CooldownDelay = _cooldown;
            }
            else
            {
                OnCooldownComplete();
            }
        }
        else
        {
            if (_cooldown > 0)
            {
                CooldownDelay = _cooldown;
            }
        }
    }

    protected virtual void OnCooldownComplete()
    {
        if (isServer)
        {
            RpcOnCooldownComplete(_skillIndex);
        }
    }

    [ClientRpc]
    void RpcOnUse(int skillIndex, GameObject unitGo, GameObject targetGo)
    {
        _skills[skillIndex]._unit = unitGo.GetComponent<Unit>();
        if (targetGo != null)
        {
            _skills[skillIndex]._target = targetGo.GetComponent<Interactable>();
        }

        _skills[skillIndex].OnUse();
    }

    [ClientRpc]
    void RpcOnCastComplete(int skillIndex)
    {
        _skills[skillIndex].OnCastComplete();
    }

    [ClientRpc]
    void RpcOnCooldownComplete(int skillIndex)
    {
        _skills[skillIndex].OnCooldownComplete();
    }
}
