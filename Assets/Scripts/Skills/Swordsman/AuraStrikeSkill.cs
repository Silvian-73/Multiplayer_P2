using UnityEngine;

public class AuraStrikeSkill : UpgradeableSkill
{
    [SerializeField] private int _baseDamage = 10;
    [SerializeField] private int _damageByLevel = 1;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _auraEffect;

    private int _damage;
    private Collider[] _bufferColliders = new Collider[64];
    private int _targetColliders;

    public override int Level
    {
        set
        {
            base.Level = value;
            _damage = _baseDamage + _damageByLevel * Level;
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
            _targetColliders = Physics.OverlapSphereNonAlloc(transform.position, _radius, _bufferColliders, _enemyMask);
            for (int i = 0; i < _targetColliders; i++)
            {
                Unit enemy = _bufferColliders[i].GetComponent<Unit>();
                if (enemy != null && enemy.HasInteract)
                {
                    enemy.TakeDamage(_unit.gameObject, _damage);
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
