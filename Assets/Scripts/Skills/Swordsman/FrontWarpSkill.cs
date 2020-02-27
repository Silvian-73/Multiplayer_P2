using UnityEngine;

public class FrontWarpSkill : Skill
{

    [SerializeField] private float _warpDistance = 7f;

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
