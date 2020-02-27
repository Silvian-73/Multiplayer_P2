using UnityEngine;

public class FrontWarpSkill : UpgradeableSkill
{

    [SerializeField] private float _warpDistance = 7f;

    public override int Level
    {
        set
        {
            base.Level = value;
            _warpDistance = 7f + 0.5f * Level;
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
