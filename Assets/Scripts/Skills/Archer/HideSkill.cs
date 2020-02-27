using UnityEngine;

public class HideSkill : UpgradeableSkill 
{

    [SerializeField] private ParticleSystem _hideEffect;

    public override int Level
    {
        set
        {
            base.Level = value;
            _castTime = Level < 10 ? 10 - Level : 1;
        }
    }

    protected override void OnUse() 
    {
        if (isServer) 
        {
            _unit.RemoveFocus();
            _unit.HasInteract = false;
        } 
        else
        {
            _hideEffect.Play();
        }
        base.OnUse();
    }

    protected override void OnCastComplete() 
    {
        if (isServer) 
        {
            _unit.HasInteract = true;
        } 
        else
        {
            _hideEffect.Stop();
        }
        base.OnCastComplete();
    }
}
