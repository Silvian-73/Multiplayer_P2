using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    public Player Player;

    private Vector3 _startPosition;
    [SerializeField] private float _reviveDelay = 5f;
    private float _reviveTime;

    [SerializeField] private GameObject gfx;

    new public PlayerStats Stats 
    { 
        get 
        { 
            return _stats as PlayerStats; 
        } 
    }

    void Start()
    {
        _startPosition = transform.position;
        _reviveTime = _reviveDelay;
    }

    void Update()
    {
        OnUpdate();
    }
    protected override void OnAliveUpdate()
    {
        base.OnAliveUpdate();
        if (_focus != null)
        {
            if (!_focus.HasInteract)
            {
                RemoveFocus();
            }
            else
            {
                float distance = Vector3.Distance(_focus.InteractionTransform.position, transform.position);
                if (distance <= _focus.Radius)
                {
                    if (!_focus.Interact(gameObject))
                    {
                        RemoveFocus();
                    }
                }
            }
        }
    }
    protected override void OnDeadUpdate()
    {
        base.OnDeadUpdate();
        if (_reviveTime > 0)
        {
            _reviveTime -= Time.deltaTime;
        }
        else
        {
            _reviveTime = _reviveDelay;
            Revive();
        }
    }
    protected override void Die()
    {
        base.Die();
        gfx.SetActive(false);
    }
    protected override void Revive()
    {
        transform.position = _startPosition;
        base.Revive();
        gfx.SetActive(true);
        if (isServer)
        {
            SetMovePoint(_startPosition);
        }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!_isDead)
        {
            _motor.MoveToPoint(point);
        }
    }
    public void SetNewFocus(Interactable newFocus)
    {
        if (!_isDead)
        {
            if (newFocus.HasInteract)
            {
                SetFocus(newFocus);
            }
        }
    }
}
