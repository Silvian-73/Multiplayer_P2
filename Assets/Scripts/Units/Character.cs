using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    public Player Player;

    private Vector3 _startPosition;
    private Vector3 _respawnPosition;
    [SerializeField] private float _reviveDelay = 5f;
    private float _reviveTime;

    [SerializeField] private GameObject gfx;

    new public PlayerStats Stats 
    { 
        get 
        { 
            return base.Stats as PlayerStats; 
        } 
    }

    void Start()
    {
        _startPosition = transform.position;
        _reviveTime = _reviveDelay;

        if (Stats.CurHealth == 0)
        {
            transform.position = _startPosition;
            if (isServer)
            {
                Stats.SetHealthRate(1);
                Motor.MoveToPoint(_startPosition);
            }
        }
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
                if (distance <= _interactDistance)
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
        transform.position = _respawnPosition;
        base.Revive();
        gfx.SetActive(true);
        if (isServer)
        {
            SetMovePoint(_respawnPosition);
        }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!_isDead)
        {
            Motor.MoveToPoint(point);
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
    public void SetRespawnPosition(Vector3 newPosition)
    {
        _respawnPosition = newPosition;
    }
}
