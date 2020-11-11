using UnityEngine;

public class ArmageddonSkill : UpgradeableSkill
{
    [SerializeField] private int _baseDamage;
    [SerializeField] private int _damagePerLevel;
    [SerializeField] private float _range;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _armageddonEffect;

    private Collider[] _colliderBuffer = new Collider[128];

    private int _damage => _baseDamage + _damagePerLevel * Level;

    protected override void OnCastComplete()
    {
        if(isServer)
        {
            int collidersCount = Physics.OverlapSphereNonAlloc(transform.position, _range, _colliderBuffer, _enemyMask);
            for (int i = 0; i < collidersCount; i++)
            {
                Unit enemy = _colliderBuffer[i].GetComponent<Unit>();
                if (enemy != null && enemy.HasInteract)
                {
                    enemy.TakeDamage(_unit.gameObject, _damage);
                }
            }
        }
        else
        {
            _armageddonEffect.Play();
        }
        base.OnCastComplete();
    }
}
