using UnityEngine;

public class MeteorStrikeSkill : UpgradeableSkill 
{

    [SerializeField] private float _range = 7f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private int _damage = 25;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _castEffect;
    [SerializeField] private ParticleSystem _meteorStrikeEffect;

    private Collider[] _bufferColliders = new Collider[64];
    private int _targetColliders;

    public override int Level
    {
        set
        {
            base.Level = value;
            _damage = 25 + 7 * Level;
            _range = Level < 3 ? 7f : 10f;
        }
    }
    protected override void Start() 
    {
        base.Start();
        _meteorStrikeEffect.transform.SetParent(null);
    }

    protected override void OnUse() 
    {
        if (isServer) 
        {
            if (_target != null && _target.GetComponent<Unit>() != null) 
            {
                if (Vector3.Distance(_target.transform.position, _unit.transform.position) <= _range) 
                {
                    _unit.RemoveFocus();
                    base.OnUse();
                }
            }
        } 
        else 
        {
            _castEffect.Play();
            base.OnUse();
        }
    }

    protected override void OnCastComplete() 
    {
        if (isServer) 
        {
            _targetColliders = Physics.OverlapSphereNonAlloc(_target.transform.position, _radius,_bufferColliders, _enemyMask);
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
            _castEffect.Stop();
            _meteorStrikeEffect.transform.position = _target.transform.position;
            _meteorStrikeEffect.transform.rotation = Quaternion.LookRotation(_target.transform.position - _unit.transform.position);
            _meteorStrikeEffect.Play();
        }
        base.OnCastComplete();
    }

    private void OnDestroy() 
    {
        if (isServer)
        {
            Destroy(_meteorStrikeEffect.gameObject);
        }
    }
}