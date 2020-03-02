using UnityEngine;

public class FrontWarpSkill : UpgradeableSkill
{

    [SerializeField] private float _baseWarpDistance = 7f;
    [SerializeField] private float _warpDistanceByLevel = 0.5f;
    private float _warpDistance;

    public override int Level
    {
        set
        {
            base.Level = value;
            _warpDistance = _baseWarpDistance + _warpDistanceByLevel * Level;
        }
    }
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
            _unit.transform.Translate(Vector3.forward * _warpDistance);
            _unit.Motor.StopFollowingTarget();
        }
        base.OnCastComplete();
    }
}
