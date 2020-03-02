using UnityEngine;

public class DoubleShotSkill : UpgradeableSkill
{

    [SerializeField] private float _range = 7f;
    [SerializeField] private int _baseDamage = 25;
    [SerializeField] private int _damageByLevel = 3;
    private int _damage;
    [SerializeField] private ParticleSystem _doubleShotEffect;

    public override int Level
    {
        set
        {
            base.Level = value;
            _damage = _baseDamage + _damageByLevel * Level;
        }
    }
    protected override void Start() 
    {
        base.Start();
        _doubleShotEffect.transform.SetParent(null);
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
            base.OnUse();
        }
    }

    protected override void OnCastComplete() 
    {
        Unit enemy = _target.GetComponent<Unit>();
        if (isServer) 
        {
            if (enemy.HasInteract) 
            {
                enemy.TakeDamage(_unit.gameObject, _damage);
                _unit.SetFocus(enemy);
            }
        } 
        else 
        {
            _doubleShotEffect.transform.position = enemy.transform.position;
            _doubleShotEffect.transform.rotation = Quaternion.LookRotation(enemy.transform.position - _unit.transform.position);
            _doubleShotEffect.Play();
        } 
        base.OnCastComplete();
    }

    private void OnDestroy() 
    {
        if (isServer)
        {
            Destroy(_doubleShotEffect.gameObject);
        }
    }
}
