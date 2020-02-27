using UnityEngine;

public class ElectroShieldSkill : UpgradeableSkill 
{

    [SerializeField] private int _damage;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _electroEffect;

    private Collider[] _bufferColliders = new Collider[64];
    private int _targetColliders;

    public override int Level
    {
        set
        {
            base.Level = value;
            _damage = 25 + 5 * Level;
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
            _targetColliders = Physics.OverlapSphereNonAlloc(transform.position, _radius,_bufferColliders, _enemyMask);
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
            _electroEffect.Play();
        }
        base.OnCastComplete();
    }

    protected void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
